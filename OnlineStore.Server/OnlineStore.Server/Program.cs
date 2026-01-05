using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineStore.Server.Authorization.Abstractions;
using OnlineStore.Server.Authorization.Implementations;
using OnlineStore.Server.Authorization.Utilities;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.Database.Seed;
using OnlineStore.Server.Extensions.BCL.Exceptions;
using OnlineStore.Server.Middleware;
using OnlineStore.Server.Repositories.Customers;
using OnlineStore.Server.Repositories.Items;
using OnlineStore.Server.Repositories.OrderElements;
using OnlineStore.Server.Repositories.Orders;
using OnlineStore.Server.Repositories.Users;
using OnlineStore.Server.Services.Customers;
using OnlineStore.Server.Services.Items;
using OnlineStore.Server.Services.OrderElements;
using OnlineStore.Server.Services.Orders;
using OnlineStore.Server.Services.Users;
using OnlineStore.Server.Utilities.Common.Database;
using OnlineStore.Server.Utilities.Order.Generators;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace OnlineStore.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json")
                                 .AddEnvironmentVariables();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Online Store Web API"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        []
                    }
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowedOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new RsaSecurityKey(KeyTool.GetPublicKey(builder.Configuration))
                };
            });

            builder.Services.AddDbContext<OnlineStoreDbContext>(options =>
            {
                options
                    .UseLazyLoadingProxies()
                    .UseNpgsql(
                        builder.Configuration["DB_CONNECTION_STRING"]
                            ?? throw new EnvironmentVariableNotFoundException("Connection string not found!"),
                        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                    .UseSnakeCaseNamingConvention();
            },
            ServiceLifetime.Scoped);

            // repo
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IItemRepository, ItemRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderElementRepository, OrderElementRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // services
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderElementService, OrderElementService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // additionally
            builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
            builder.Services.AddScoped<ExceptionCatcherMiddleware>();
            builder.Services.AddScoped<INumberGenerator, OrderNumberGenerator>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();

            // logging
            builder.Logging.AddDebug()
                           .AddConsole();

            var app = builder.Build();

            app.UseMiddleware<ExceptionCatcherMiddleware>();

            // auto migrate
            using (var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<OnlineStoreDbContext>();
                await context.Database.MigrateAsync();
                await Seeder.StartSeed(context);
            }

            // swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
                options.DocExpansion(DocExpansion.None);
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("AllowedOrigins");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
