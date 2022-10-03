using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class AddAuthorResponse : BaseResponse
    {
        public Author? Author { get; init; } 
    }
}
