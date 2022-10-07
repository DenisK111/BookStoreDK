using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookStoreDK.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    AppException e => (int)HttpStatusCode.BadRequest,
                    KeyNotFoundException e => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var result = new
                {
                    message = error.Message
                };
                _logger.LogError(JsonConvert.SerializeObject(result));
                await response.WriteAsJsonAsync(result);
                               
            }

        }
    }
}
