using System.Threading.Tasks.Dataflow;
using BookStoreDK.BL.Dataflow;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.KafkaCache;
using BookStoreDK.Models.Configurations;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Models.KafkaConsumerModels;

namespace BookStoreDK.Dataflow
{
    public class DeliveryDataFlow : DataFlow
    {
        private readonly KafkaConsumer<int, BookDeliveryObject, KafkaBookDeliveryConsumerSettings> _kafkaConsumer;
        private readonly IBookRepository _bookRepository;
        private readonly TransformBlock<BookDeliveryObject, Dictionary<int, int>> _getQuantityBlock;
        private readonly ActionBlock<Dictionary<int, int>> _writeToDatabaseBlock;

        public DeliveryDataFlow(KafkaConsumer<int, BookDeliveryObject, KafkaBookDeliveryConsumerSettings> kafkaConsumer, IBookRepository bookRepository)
        {

            _bookRepository = bookRepository;

            var options = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 4,
                BoundedCapacity = 4
            };

            _getQuantityBlock = new TransformBlock<BookDeliveryObject, Dictionary<int, int>>(GetIdValuePairs, options);
            _writeToDatabaseBlock = new ActionBlock<Dictionary<int, int>>(WriteToDb, options);

            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true,
            };

            _getQuantityBlock.LinkTo(_writeToDatabaseBlock, linkOptions);
            _kafkaConsumer = kafkaConsumer;
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

       protected override async Task StartDataFlow(CancellationToken cancellationToken)
        {
            Action<BookDeliveryObject> postMessage = async x => await _getQuantityBlock.SendAsync(x);
            await _kafkaConsumer.HandleMessages(postMessage, cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }

        private async Task<Dictionary<int, int>> GetIdValuePairs(BookDeliveryObject delivery)
        {
            var result = new Dictionary<int, int>();

            var item = delivery.Book;
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

            return result;
        }

        private async Task WriteToDb(Dictionary<int, int> bookIdQuantityPairs)
        {
            foreach (var (id, qty) in bookIdQuantityPairs)
            {                
                var book = await _bookRepository.GetById(id);                              
                var updatedBook = new Book()
                {
                    Id = book.Id,
                    Title = book.Title,
                    AuthorId = book.AuthorId,
                    Quantity = book.Quantity + qty,
                    Price = book.Price,
                };               
                await _bookRepository.Update(updatedBook);
            }
        }
    }
}
