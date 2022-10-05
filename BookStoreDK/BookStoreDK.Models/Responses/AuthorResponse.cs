using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class AuthorResponse : BaseResponse
    {
        public Author? Author { get; init; }
    }
}
