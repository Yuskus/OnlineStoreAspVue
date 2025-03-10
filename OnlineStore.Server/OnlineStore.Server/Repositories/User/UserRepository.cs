using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Authorization.Utilities;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Mapping.User;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.User
{
    public class UserRepository(OnlineStoreDbContext context, IConfiguration configuration, ILogger<UserRepository> logger) : IUserRepository
    {
        private readonly OnlineStoreDbContext _context = context;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<UserRepository> _logger = logger;

        public async Task<LoginResponse?> Authenticate(UserCredentialsRequest loginRequest)
        {
            if (await _context.Users.FirstOrDefaultAsync(x => x.Username == loginRequest.Username) is Entity.User user)
            {
                bool isValid = Hasher.IsPasswordValid(loginRequest.Password, user.Password, user.Salt);

                if (isValid)
                {
                    LoginResponse response = user.MapAuthFromDb();

                    TokenGenerator.GenerateJwtToken(_configuration, response);

                    return response;
                }

                _logger.LogWarning("Неудачная попытка входа!");
            }

            return null;
        }

        public async Task<bool> Create(UserCredentialsRequest registerRequest)
        {
            if (await _context.Users.AnyAsync(x => x.Username == registerRequest.Username)) return false;

            (byte[] hash, byte[] salt) = Hasher.CreatePasswordHash(registerRequest.Password);

            Entity.User userEntity = registerRequest.MapUserToDb(hash, salt);

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(string username, UserRequest userRequest)
        {
            if (await _context.Users.FirstOrDefaultAsync(x => x.Username == username) is Entity.User user)
            {
                user.UpdateInDb(userRequest); // меняем только роль и логин (логика смены пароля не добавлена)
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> Delete(string username)
        {
            if (await _context.Users.FirstOrDefaultAsync(x => x.Username == username) is Entity.User user)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                
                return true;
            }

            return false;
        }

        public async Task<ResponseList<UserResponse>> GetAll()
        {
            return new()
            {
                Responses = await _context.Users.Include(x => x.Customer).Select(x => x.MapFromDb()).ToListAsync(),
                TotalCount = await _context.Users.CountAsync()
            };
        }
    }
}
