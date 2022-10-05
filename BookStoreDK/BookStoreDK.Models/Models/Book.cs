namespace BookStoreDK.Models.Models
{
    public record Book : BaseIdModel<int>
    {
        public string Title { get; init; } = null!;
        public int AuthorId { get; init; }

        public int Quantity { get; init; }

        public DateTime LastUpdated { get; init; }

        public decimal Price { get; init; }
    }
}
