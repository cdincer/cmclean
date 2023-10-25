namespace cmclean.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        readonly ILogger _log;
        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory) 
        {
            _next = next;
            _log = loggerFactory.CreateLogger(GetType());
        }

        public async Task Invoke(HttpContext Context)
        {
            try
            {
                await _next(Context);
                _log.LogInformation("Executed request "+ Context.Request.Path +" Method "+Context.Request.Method);
            }
            catch(Exception error)
            {
                var response = Context.Response;
                response.ContentType = "application/json";
                _log.LogError(error, "Error handling the path " + Context.Request.Path);
                await response.WriteAsync("Error");
            }
        }
    }

    public static class ExceptionLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
