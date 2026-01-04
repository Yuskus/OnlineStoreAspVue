namespace OnlineStore.Server.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddExceptionCatcher(this IServiceCollection builder)
        {
            return builder.AddScoped<ExceptionCatcherMiddleware>();
        }

        public static IApplicationBuilder UseExceptionCatcher(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionCatcherMiddleware>();
        }
    }
}