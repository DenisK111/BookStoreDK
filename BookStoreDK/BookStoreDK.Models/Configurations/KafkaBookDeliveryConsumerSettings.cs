using BookStoreDK.KafkaCache.Contracts;

namespace BookStoreDK.Models.Configurations
{
    public class KafkaBookDeliveryConsumerSettings : IKafkaConsumerSettings
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
        public string GroupId { get; set; }
    }
}
