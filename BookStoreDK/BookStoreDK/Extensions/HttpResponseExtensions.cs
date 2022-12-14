using System.Net;
using BookStoreDK.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreDK.Extensions
{
    public static class HttpResponseExtensions
    {
        public static IActionResult ProduceResponse<T>(this ControllerBase controller, BaseResponse<T> response)
        {
            switch (response.HttpStatusCode)
            {
                case HttpStatusCode.OK:
                    return controller.Ok(response);
                case HttpStatusCode.NotFound:
                    return controller.NotFound(response);
                case HttpStatusCode.BadRequest:
                    return controller.BadRequest(new ErrorResponse() {Error=response.Message});
                default: return controller.StatusCode(500);
            }
        }
    }
}
