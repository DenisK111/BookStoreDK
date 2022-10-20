namespace BookStoreDK.KafkaCache.Contracts
{
    public interface ICacheItem<T> : IKafkaItem<T>
    {
        public DateTime LastUpdated { get; init; }
        T GetKey();       
    }
}
