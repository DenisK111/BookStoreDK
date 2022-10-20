using BookStoreDK.KafkaCache.Contracts;
using Commons;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace BookStoreDK.KafkaCache
{
    public class KafkaConsumer<TKey, TValue, TSettings>
        where TSettings : IKafkaConsumerSettings
        where TValue : class, IKafkaItem<TKey>
        where TKey : notnull
    {

        private readonly IOptionsMonitor<TSettings> _kafkaSettings;
        private readonly IConsumer<TKey, TValue> _consumer;
        public KafkaConsumer(IOptionsMonitor<TSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings;
            _consumer = new ConsumerBuilder<TKey, TValue>(new ConsumerConfig()
            {
                BootstrapServers = kafkaSettings.CurrentValue.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = $"{typeof(TValue).Name}{kafkaSettings.CurrentValue.GroupId}"
            })
                .SetKeyDeserializer(new MsgDeserializer<TKey>())
                .SetValueDeserializer(new MsgDeserializer<TValue>())
                .Build();
            _consumer.Subscribe($"{typeof(TValue).Name}{kafkaSettings.CurrentValue.Topic}");
        }

        public Task HandleMessages(Action<TValue> handleMessageAction, CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = _consumer.Consume(cancellationToken);
                    var value = cr.Message.Value;
                    handleMessageAction.Invoke(value);
                }
            });

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}
