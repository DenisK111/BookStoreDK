namespace BookStoreDK.KafkaCache.Contracts
{
    public interface ICacheItem<out T>
    {
        public DateTime LastUpdated { get; init; }
        T GetKey();

       
    }
}
