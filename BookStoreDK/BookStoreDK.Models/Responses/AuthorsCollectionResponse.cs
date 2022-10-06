using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class AuthorsCollectionResponse : BaseResponse
    {
        public IEnumerable<Author> Authors { get; init; }
    }
}
