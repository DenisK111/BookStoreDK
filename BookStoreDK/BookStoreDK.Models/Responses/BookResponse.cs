using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class BookResponse : BaseResponse
    {
        public Book? Book { get; init; }
    }
}
