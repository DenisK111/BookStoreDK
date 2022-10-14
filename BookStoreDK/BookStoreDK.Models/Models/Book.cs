using BookStoreDK.KafkaCache.Contracts;
using MessagePack;

namespace BookStoreDK.Models.Models
{
    [MessagePackObject]
    public record Book : ICacheItem<int>
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Title { get; init; } = null!;
        [Key(3)]
        public int AuthorId { get; init; }
        [Key(2)]
        public int Quantity { get; init; }
        [Key(4)]
        public DateTime LastUpdated { get; init; }
        [Key(5)]
        public decimal Price { get; init; }

        public int GetKey()
        {
            return Id;
        }

    }
}
