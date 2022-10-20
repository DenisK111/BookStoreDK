using System.Threading.Tasks.Dataflow;
using BookStoreDK.BL.Dataflow;
using BookStoreDK.BL.HttpClientProviders.Contracts;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.KafkaCache;
using BookStoreDK.Models.Configurations;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Models.KafkaConsumerModels;

namespace BookStoreDK.Dataflow
{
    public class PurchaseDataFlow : DataFlow
    {
        private readonly KafkaConsumer<Guid, PurchaseObject, KafkaPurchaseConsumerSettings> _kafkaConsumer;
        private readonly IBookRepository _bookRepository;
        private readonly TransformBlock<PurchaseObject, Dictionary<int, int>> _getQuantityBlock;
        private readonly ActionBlock<Dictionary<int, int>> _writeToDatabaseBlock;
        private readonly IAdditionalInfoProvider _additionalInfoProvider;

        public PurchaseDataFlow(KafkaConsumer<Guid, PurchaseObject, KafkaPurchaseConsumerSettings> kafkaConsumer, IBookRepository bookRepository, IAdditionalInfoProvider additionalInfoProvider)
        {
            _additionalInfoProvider = additionalInfoProvider;
            _bookRepository = bookRepository;

            var options = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 4,
                BoundedCapacity = 4
            };

            _getQuantityBlock = new TransformBlock<PurchaseObject, Dictionary<int, int>>(GetIdValuePairs, options);
            _writeToDatabaseBlock = new ActionBlock<Dictionary<int, int>>(WriteToDb, options);

            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true,
            };

            _getQuantityBlock.LinkTo(_writeToDatabaseBlock, linkOptions);
            _kafkaConsumer = kafkaConsumer;
        }

        protected override async Task StartDataFlow(CancellationToken cancellationToken)
        {
            Action<PurchaseObject> postMessage = async x => await _getQuantityBlock.SendAsync(x);
            await _kafkaConsumer.HandleMessages(postMessage, cancellationToken);
        }

        private async Task<Dictionary<int, int>> GetIdValuePairs(PurchaseObject purchase)
        {
            var result = new Dictionary<int, int>();
            var authorIds = purchase.Books.Select(x => x.AuthorId).Distinct();

            var authorInfoListTasks = new List<Task<string>>();

            foreach (var authorId in authorIds)
            {
                var t = Task<string>.Run(async () =>
                {
                    var info = await _additionalInfoProvider.GetAdditionalInfo(authorId);
                    return info;
                });

                authorInfoListTasks.Add(t);
            }

            purchase.AdditionalInfo = await Task.WhenAll(authorInfoListTasks);                       
                      
            foreach (var item in purchase.Books)
            {
                var id = item.Id;
                var quantity = item.Quantity;

                var bookEntity = await _bookRepository.GetById(id);

                if (bookEntity == null)
                {
                    var book = new Book()
                    {
                        Id = id,
                        Quantity = 0,
                        Price = item.Price,
                        AuthorId = item.AuthorId,
                        Title = item.Title,
                    };
                    await _bookRepository.Add(book);
                }

                if (!result.ContainsKey(id))
                {
                    result[id] = 0;
                }

                result[id] += quantity;
            }

            return result;
        }

        private async Task WriteToDb(Dictionary<int, int> bookIdQuantityPairs)
        {
            foreach (var (id, qty) in bookIdQuantityPairs)
            {
                var book = await _bookRepository.GetById(id);

                if (book!.Quantity - qty <= 0)
                {
                    continue;
                }

                var updatedBook = new Book()
                {
                    Id = book!.Id,
                    Title = book.Title,
                    AuthorId = book.AuthorId,
                    Quantity = book.Quantity - qty,
                    Price = book.Price,
                };
                await _bookRepository.Update(updatedBook);
            }
        }

        public override void Dispose()
        {
            _kafkaConsumer.Dispose();
            _getQuantityBlock.Complete();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await StartDataFlow(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
