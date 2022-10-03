using System.Net;

namespace BookStoreDK.Models.Responses
{
    public abstract class BaseResponse
    {
        public HttpStatusCode HttpStatusCode { get; init; }

        public string Message { get; set; } = null!;
    }
}
