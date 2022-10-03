namespace BookStoreDK.Models.Requests
{
    public class AddBookRequest 
    {
        public string Title { get; init; } = null!;
        public int AuthorId { get; init; }
    }
}
