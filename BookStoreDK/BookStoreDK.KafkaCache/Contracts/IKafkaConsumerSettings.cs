namespace BookStoreDK.KafkaCache.Contracts
{
    public interface IKafkaConsumerSettings
    {
        public string BootstrapServers { get; set; }      
        public string Topic { get; set; }
        public string GroupId { get; set; }
    }
}
