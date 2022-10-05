namespace BookStoreDK.Models.Requests
{
    public class AddBookRequest
    {
        public string Title { get; init; } = null!;
        public int AuthorId { get; init; }

        public int Quantity { get; init; }

        public decimal Price { get; init; }
    }
}
