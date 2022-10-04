using BookStoreDK.Models.Models;

namespace BookStoreDK.Models.Responses
{
    public class AddPersonResponse : BaseResponse
    {
        public Person? Person { get; set; }
    }
}