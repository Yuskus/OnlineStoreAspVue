using System.Text;

namespace OnlineStore.Server.Middleware
{
    public class ExceptionCatcherMiddleware(ILogger<ExceptionCatcherMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<ExceptionCatcherMiddleware> _logger = logger;
        private readonly HashSet<string> excludedPaths =
        [
            "login",
            "registercustomer",
            "registermanager",
        ];
        private readonly HashSet<string> methodTypes =
        [
            "POST",
            "PUT"
        ];

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string? body = null;

            try
            {
                body = await TryGetBody(context.Request);

                await next(context);
            }
            catch (Exception exception)
            {
                HandleError(exception, context.Request, body);

                await SetResponse(exception, context.Response);
            }
        }

        public async Task<string?> TryGetBody(HttpRequest request)
        {
            var method = request.Method;
            var path = request.Path.Value?.ToLower();

            if (method == null)
                return null;
            if (path == null)
                return null;
            if (!methodTypes.Contains(method))
                return null;
            if (excludedPaths.Contains(path))
                return null;

            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            string? body = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            return body;
        }

        public void HandleError(Exception exception, HttpRequest request, string? body)
        {
            _logger.LogError(exception,
                "Возникло исключение: \n" +
                "Источник исключения: {Source}\n" +
                "Сообщение исключения: {Message}\n" +
                "Стек (выборочно): {Stack}\n" +
                "Метод: {Method}\n" +
                "Путь: {Path}\n" +
                "Тело: {Body}\n" +
                "Параметры Query: {QueryParams}\n",
                    exception.Source,
                    exception.Message,
                    string.Join('\n', exception.StackTrace?.Split('\n').Where(x => x.Contains("cs:line")) ?? []),
                    request.Method,
                    request.Path,
                    body ?? "no body or confidential",
                    string.Join(", ", request.Query?.Select(x => $"{x.Key} = {x.Value}") ?? []));
        }

        public static async Task SetResponse(Exception exception, HttpResponse response)
        {
            response.ContentType = "application/json";
            response.StatusCode = StatusCodes.Status400BadRequest;

            await response.WriteAsJsonAsync(new
            {
                response.StatusCode,
                exception.Message
            });
        }
    }
}