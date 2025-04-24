using Newtonsoft.Json;
using System.Text;

namespace OnlineStore.Server.Middleware
{
    public class ExceptionCatcherMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionCatcherMiddleware> _logger;
        private readonly HashSet<string> pathsWithDisabledBuffering = new()
        {
            "пути-исключения",
            "по которым мы не логируем body",
            "например, login и тп"
        };

        public ExceptionCatcherMiddleware(ILogger<ExceptionCatcherMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string? body = null;

            try
            {
                if ((context.Request.Method == "POST" || context.Request.Method == "PUT") &&
                    !pathsWithDisabledBuffering.Contains(context.Request.Path))
                {
                    context.Request.EnableBuffering();

                    using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                    body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }

                await next(context);
            }
            catch (Exception exception)
            {
                var keyStackLines = string.Join('\n', exception.StackTrace?.Split('\n').Where(x => x.Contains("cs:line")) ?? []);
                var parameters = string.Join(", ", context.Request.Query?.Select(x => $"{x.Key} = {x.Value}") ?? []);

                _logger.LogError(exception, "Возникло исключение: ");

                _logger.LogError("Источник исключения: {Source}", exception.Source);
                _logger.LogError("Сообщение исключения: {Message}", exception.Message);
                _logger.LogError("Стек (выборочно): {Stack}", keyStackLines);
                _logger.LogError("Метод: {Method}", context.Request.Method);
                _logger.LogError("Путь: {Path}", context.Request.Path);
                _logger.LogError("Тело: {Body}", body ?? "no body or confidential");
                _logger.LogError("Параметры Query: {QueryParams}", parameters);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    context.Response.StatusCode,
                    exception.Message
                }));
            }
        }
    }
}