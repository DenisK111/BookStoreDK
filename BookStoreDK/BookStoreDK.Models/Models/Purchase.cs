namespace BookStoreDK.Models.Models
{

    public record Purchase
    {        
        public Guid Id { get; set; }

        public IEnumerable<Book> Books { get; set; } = Enumerable.Empty<Book>().ToList();

        public decimal TotalMoney { get; set; }

        public int UserId { get; set; }
    }
}
