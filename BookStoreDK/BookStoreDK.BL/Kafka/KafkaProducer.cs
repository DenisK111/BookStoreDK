using BookStoreDK.Models.Configurations;
using Commons;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace BookStoreDK.BL.Kafka
{
    public class KafkaProducer<TKey, TValue>
    {
        private readonly IOptionsMonitor<KafkaProducerSettings> _kafkaSettings;
        private readonly IProducer<TKey, TValue> _producer;
        public KafkaProducer(IOptionsMonitor<KafkaProducerSettings> kafkaSettings)
        {

            _kafkaSettings = kafkaSettings;
            _producer = new ProducerBuilder<TKey, TValue>(new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.CurrentValue.BootstrapServers
            })
                .SetValueSerializer(new MsgSerializer<TValue>())
                .Build();
        }

        public async Task<DeliveryResult<TKey,TValue>> Produce(TKey key,TValue value)
        {

            var msg = new Message<TKey, TValue>()
            {
                Key = key,
                Value = value,
            };

            return await _producer.ProduceAsync(_kafkaSettings.CurrentValue.Topic, msg);
        }


    }
}
