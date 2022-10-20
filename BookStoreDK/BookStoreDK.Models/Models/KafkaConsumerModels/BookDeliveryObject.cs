using BookStoreDK.KafkaCache.Contracts;
using MessagePack;

namespace BookStoreDK.Models.Models.KafkaConsumerModels
{
    [MessagePackObject]
    public record BookDeliveryObject :IKafkaItem<int>
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public BookConsumeObject Book { get; set; }
        [Key(2)]
        public int Quantity { get; set; }
       
    }
}
