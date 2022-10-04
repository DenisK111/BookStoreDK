using System.Net;
using BookStoreDK.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreDK.Extensions
{
    public static class HttpResponseExtensions
    {
        public static IActionResult ProduceResponse(this ControllerBase controller, BaseResponse response)
        {
            switch (response.HttpStatusCode)
            {
                case HttpStatusCode.OK:
                    return controller.Ok(response);
                case HttpStatusCode.NotFound:
                    return controller.NotFound(response);
                default: return controller.BadRequest(new {error = response.Message});
            }
        }
    }
}
