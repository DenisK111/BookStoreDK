using BookStoreDK.BL.Kafka;
using BookStoreDK.Models.Models;

namespace BookStoreDK.HostedServices
{
    public class KafkaConsumersHostedService : IHostedService
    {
        private readonly KafkaConsumer<int, Book> _kafkaConsumer;

        public KafkaConsumersHostedService(KafkaConsumer<int,Book> kafkaConsumer)
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
            return Task.CompletedTask;
        }
    }
}
