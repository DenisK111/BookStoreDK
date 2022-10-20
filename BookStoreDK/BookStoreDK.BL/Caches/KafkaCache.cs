using System.Reflection;
using BookStoreDK.KafkaCache.Contracts;

namespace BookStoreDK.KafkaCache
{
    public class KafkaCache<TKey, TValue, TSettings> : IDisposable
        where TSettings : IKafkaConsumerSettings
        where TValue : class, IKafkaItem<TKey>, ICacheItem<TKey>
        where TKey : notnull

    {

        private Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();
        private List<CachedIndex<TValue>> _indexes;
        private readonly KafkaConsumer<TKey, TValue, TSettings> _consumer;


        public IReadOnlyDictionary<TKey, TValue> ConsumerDictionary => _cache;
        public IReadOnlyCollection<CachedIndex<TValue>> CachedIndexes => _indexes.AsReadOnly();


        public KafkaCache(KafkaConsumer<TKey, TValue, TSettings> consumer)
        {
            _consumer = consumer;
            _indexes = new List<CachedIndex<TValue>>()
        {
            new CachedIndex<TValue>()
            {
                Name = "Id",
                Index = _cache.Values.ToDictionary((x) => (object)x.GetKey(),y => y)        }
    };
        }

        private void HandleMessageAction(TValue messageValue)
        {
            var key = messageValue.GetKey();
            var value = messageValue;

            if (!_cache.ContainsKey(key))
            {
                _cache[key] = value;
            }

            if (value.LastUpdated > _cache[key].LastUpdated)
            {
                _cache[key] = value;
            }
            foreach (var item in _indexes)
            {
                Type t = typeof(TValue);

                PropertyInfo prop = t.GetProperty(item.Name)!;
                var propValue = prop.GetValue(value);

                if (!item.Index.ContainsKey(propValue!))
                {
                    item.Index[propValue!] = value;
                }

                if (value.LastUpdated > item.Index[propValue!].LastUpdated)
                {
                    item.Index[propValue!] = value;
                }

            }
        }

        public Task StartCache(CancellationToken cancellationToken)
        {
            _consumer.HandleMessages(HandleMessageAction, cancellationToken);
            return Task.CompletedTask;
        }

        public (bool isSuccess, string message) AddIndex(string propertyName)
        {
            if (_indexes.Select(x => x.Name).Contains(propertyName))
            {
                return (false, "Index already Exists");
            }

            Type t = typeof(TValue);
            PropertyInfo prop = t.GetProperty(propertyName)!;

            if (prop == null)
            {
                return (false, "No such property exists");
            }

            _indexes.Add(new CachedIndex<TValue>()
            {
                Name = propertyName,
                Index = _cache.Values.ToDictionary((x) =>
                {
                    var value = prop.GetValue(x);
                    return value!;
                }, y => y)
            });

            return (true, "");
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}
