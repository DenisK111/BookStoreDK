using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Configurations;
using BookStoreDK.Models.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreDK.DL.Repositories.MongoRepositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IMongoCollection<ShoppingCart> _collection;

        public ShoppingCartRepository(IOptionsMonitor<MongoDbConfiguration> settings)
        {
            var client = new MongoClient(settings.CurrentValue.ConnectionString);
            var database = client.GetDatabase(settings.CurrentValue.DatabaseName);
            _collection = database.GetCollection<ShoppingCart>(settings.CurrentValue.ShoppingCartCollectionName);
        }

        public async Task<ShoppingCart> AddToCart(Book book, int userId)
        {
            var document = await (await _collection.FindAsync(x => x.UserId == userId)).SingleOrDefaultAsync();

            if (document == null)
            {
                var books = new List<Book>()
                    {
                        book
                    };

                var result = new ShoppingCart()
                {
                    UserId = userId,
                    Books = books
                };

                await _collection.InsertOneAsync(result);
                return result;
            }

            else
            {
                var books = document!.Books.ToList();
                books.Add(book);
                var update = Builders<ShoppingCart>.Update.Set(x => x.Books, books);

                var result = await _collection.UpdateOneAsync(x => x.UserId == userId, update);
                return await (await _collection.FindAsync(x => x.UserId == userId)).SingleOrDefaultAsync();
            }


        }

        public async Task EmptyCart(int userId)
        {
            await _collection.DeleteOneAsync(x => x.UserId == userId);
        }

        public async Task<ShoppingCart> GetPurchase(int userId)
        {
            var result = await (await _collection.FindAsync(x => x.UserId == userId)).SingleOrDefaultAsync();
            return result;
        }

        public async Task<(ShoppingCart cart, long deletedCount)> RemoveFromCart(int bookId, int userId)
        {
            var document = await (await _collection.FindAsync(x => x.UserId == userId)).SingleOrDefaultAsync();

            if (document == null)
            {
                return (new ShoppingCart()
                {
                    Books = new List<Book>(),
                    UserId = userId
                }, 0);
            }

            else
            {
                var books = document!.Books.ToList();
                var bookToRemove = books.FirstOrDefault(x => x.Id == bookId);

                if (bookToRemove == null)
                {
                    return (document, 0);
                }

                books.Remove(bookToRemove);
                var update = Builders<ShoppingCart>.Update.Set(x => x.Books, books);

                await _collection.UpdateOneAsync(x => x.UserId == userId, update);
                long result = 0;
                if (!books.Any())
                {
                    result = (await _collection.DeleteOneAsync(x => x.UserId == userId)).DeletedCount;
                }

                return (await (await _collection.FindAsync(x => x.UserId == userId)).SingleOrDefaultAsync(), result);
            }
        }
    }
}
