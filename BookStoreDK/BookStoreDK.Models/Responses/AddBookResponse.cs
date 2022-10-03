using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class AddBookResponse : BaseResponse
    {
        public Book? Book { get; init; }
    }
}
