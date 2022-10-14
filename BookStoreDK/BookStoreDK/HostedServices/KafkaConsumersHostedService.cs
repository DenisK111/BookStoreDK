
using BookStoreDK.KafkaCache;
using BookStoreDK.Models.Configurations;
using BookStoreDK.Models.Models;

namespace BookStoreDK.HostedServices
{
    public class KafkaConsumersHostedService : IHostedService
    {
        private readonly KafkaConsumer<int, Book,KafkaBookConsumerSettings> _kafkaConsumer;

        public KafkaConsumersHostedService(KafkaConsumer<int,Book, KafkaBookConsumerSettings> kafkaConsumer)
        {
            _kafkaConsumer = kafkaConsumer;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _kafkaConsumer.Consume(cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _kafkaConsumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
