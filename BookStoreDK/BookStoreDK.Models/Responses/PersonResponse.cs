using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class PersonResponse : BaseResponse
    {
        public Person? Person { get; set; }
    }
}