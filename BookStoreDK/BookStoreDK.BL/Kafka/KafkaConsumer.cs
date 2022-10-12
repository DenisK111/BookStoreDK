using BookStoreDK.Models.Configurations;
using Commons;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace BookStoreDK.BL.Kafka
{
    public class KafkaConsumer<TKey,TValue>
    {
        private readonly IOptionsMonitor<KafkaConsumerSettings> _kafkaSettings;
        private readonly IConsumer<TKey, TValue> _consumer;
        private  Dictionary<TKey,TValue> _dictionary = new Dictionary<TKey,TValue>();

        public IReadOnlyDictionary<TKey, TValue> ConsumerDictionary => _dictionary;

        public KafkaConsumer(IOptionsMonitor<KafkaConsumerSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings;
            _consumer = new ConsumerBuilder<TKey, TValue>(new ConsumerConfig()
            {
                BootstrapServers = _kafkaSettings.CurrentValue.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = _kafkaSettings.CurrentValue.GroupId
            })
                .SetValueDeserializer(new MsgDeserializer<TValue>())
                .Build();
            _consumer.Subscribe(_kafkaSettings.CurrentValue.Topic);
        }

        public Task Consume(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = _consumer.Consume();
                    _dictionary[cr.Message.Key] = cr.Message.Value;                    
                }
            });

            return Task.CompletedTask;
        }
    }
}
