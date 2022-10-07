using System.Net;

namespace BookStoreDK.Models.Responses
{
    public abstract class BaseResponse<T>
    {
        public HttpStatusCode HttpStatusCode { get; init; }

        public string Message { get; set; } = null!;

        public T? Model { get; set; } = default(T)!;
    }
}
