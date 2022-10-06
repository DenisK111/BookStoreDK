using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class BookCollectionResponse : BaseResponse
    {
         public IEnumerable<Book> Books { get; init; }
    }
}
