
using BookStoreDK.KafkaCache;
using BookStoreDK.Models.Configurations;
using BookStoreDK.Models.Models;

namespace BookStoreDK.HostedServices
{
    public class KafkaBookCacheHostedService : IHostedService
    {
        private readonly KafkaCache<int, Book,KafkaBookConsumerSettings> _kafkaBookCache;

        public KafkaBookCacheHostedService(KafkaCache<int,Book, KafkaBookConsumerSettings> kafkaCache)
        {
            _kafkaBookCache = kafkaCache;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _kafkaBookCache.StartCache(cancellationToken);                        
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _kafkaBookCache.Dispose();
            return Task.CompletedTask;
        }
    }
}
