using BookStoreDK.KafkaCache.Contracts;

namespace BookStoreDK.KafkaCache
{
    public class CachedIndex<TValue>
    {
        public string Name { get; set; }

        public Dictionary<object,TValue> Index { get; set; }
    }
}