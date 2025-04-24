namespace OnlineStore.Server.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionCatcher(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionCatcherMiddleware>();
        }

        public static IServiceCollection AddExceptionCatcher(this IServiceCollection builder)
        {
            return builder.AddScoped<ExceptionCatcherMiddleware>();
        }
    }
}