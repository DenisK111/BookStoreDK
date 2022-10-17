using System.Reflection;
using BookStoreDK.KafkaCache.Contracts;
using Commons;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace BookStoreDK.KafkaCache
{
    public class KafkaConsumer<TKey, TValue, TSettings> : IDisposable
        where TSettings : IKafkaConsumerSettings
        where TValue : class, ICacheItem<TKey>
        where TKey : notnull
    {
        private readonly IOptionsMonitor<TSettings> _kafkaSettings;
        private readonly IConsumer<TKey, TValue> _consumer;
        private Dictionary<TKey, TValue> _bookCache = new Dictionary<TKey, TValue>();
        private List<CachedIndex<TValue>> _indexes;


        public IReadOnlyDictionary<TKey, TValue> ConsumerDictionary => _bookCache;
        public IReadOnlyCollection<CachedIndex<TValue>> CachedIndexes => _indexes.AsReadOnly();


        public KafkaConsumer(IOptionsMonitor<TSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings;
            _consumer = new ConsumerBuilder<TKey, TValue>(new ConsumerConfig()
            {
                BootstrapServers = _kafkaSettings.CurrentValue.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = $"{typeof(TValue).Name}ConsumerGroup"
            })
                .SetKeyDeserializer(new MsgDeserializer<TKey>())
                .SetValueDeserializer(new MsgDeserializer<TValue>())
                .Build();
            _consumer.Subscribe($"{typeof(TValue).Name}Cache");


            _indexes = new List<CachedIndex<TValue>>()
        {
            new CachedIndex<TValue>()
            {
                Name = "Id",
                Index = _bookCache.Values.ToDictionary((x) => (object)x.GetKey(),y => y)        }
    };
        }

        public Task Consume(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = _consumer.Consume();
                    var key = cr.Message.Key;
                    var value = cr.Message.Value;
                    Console.WriteLine($"{key}:{value}");

                    if (!_bookCache.ContainsKey(key))
                    {
                        _bookCache[key] = value;
                    }

                    if (value.LastUpdated > _bookCache[key].LastUpdated)
                    {
                        _bookCache[key] = value;
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
            });

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }

        public (bool isSuccess, string message) AddIndex(string propertyName)
        {
            if (_indexes.Select(x=>x.Name).Contains(propertyName))
            {
                return (false,"Index already Exists");
            }

            Type t = typeof(TValue);
            PropertyInfo prop = t.GetProperty(propertyName)!;

            if (prop == null)
            {
                return (false,"No such property exists");
            }

            _indexes.Add(new CachedIndex<TValue>()
            {
                Name = propertyName,
                Index = _bookCache.Values.ToDictionary((x) =>
                {                    
                    var value = prop.GetValue(x);
                    return value!;
                }, y => y)
            });

            return (true,"");
        }

    }
}
