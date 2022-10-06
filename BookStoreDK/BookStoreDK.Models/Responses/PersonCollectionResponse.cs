using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class PersonCollectionResponse : BaseResponse
    {
        public IEnumerable<Person> People { get; init; }
    }
}
