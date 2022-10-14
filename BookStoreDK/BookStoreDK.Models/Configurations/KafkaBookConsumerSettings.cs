using BookStoreDK.KafkaCache.Contracts;

namespace BookStoreDK.Models.Configurations
{
    public class KafkaBookConsumerSettings : IKafkaConsumerSettings
    {
        public string BootstrapServers { get; set; }       
    }
}
