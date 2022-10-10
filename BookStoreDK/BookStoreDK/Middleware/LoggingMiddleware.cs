using Newtonsoft.Json;

namespace BookStoreDK.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var httpMethod = context.Request.Method;

            if (httpMethod == "GET")
            {
                var logEnter = new
                {
                    RequestType = httpMethod,

                    Route = context.Request.Path + context.Request.QueryString,

                    Message = "Enterin GET Request"
                };

                _logger.LogError(JsonConvert.SerializeObject(logEnter, Formatting.Indented));
            }

            else
            {
                var logEnter = new
                {
                    RequestType = httpMethod,

                    Route = context.Request.Path + context.Request.QueryString,

                    Message = $"Entering {httpMethod} Request",

                    Message2 = "This is not a get Request"
                };

                _logger.LogError(JsonConvert.SerializeObject(logEnter, Formatting.Indented));
            }


            await _next(context);


            var responseLogger = new
            {
                ResponseContentType = context.Response.ContentType,
            };

            _logger.LogCritical(JsonConvert.SerializeObject(responseLogger, Formatting.Indented));
        }
    }
}