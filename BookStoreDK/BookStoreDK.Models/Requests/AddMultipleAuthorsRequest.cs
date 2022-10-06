namespace BookStoreDK.Models.Requests
{
    public class AddMultipleAuthorsRequest
    {
        public IEnumerable<AddAuthorRequest> AuthorRequests { get; set; }

        public string Reason { get; set; }
    }
}
