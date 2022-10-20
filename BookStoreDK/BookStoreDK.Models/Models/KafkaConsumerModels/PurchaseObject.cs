using BookStoreDK.KafkaCache.Contracts;
using MessagePack;

namespace BookStoreDK.Models.Models.KafkaConsumerModels
{
    [MessagePackObject]
    public record PurchaseObject : IKafkaItem<Guid>
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public IEnumerable<BookConsumeObject> Books { get; set; } = Enumerable.Empty<BookConsumeObject>().ToList();
        [Key(2)]
        public decimal TotalMoney { get; set; }
        [Key(3)]
        public int UserId { get; set; }
        [Key(4)]
        public IEnumerable<string> AdditionalInfo { get; set; } = Enumerable.Empty<string>();
    }
}
