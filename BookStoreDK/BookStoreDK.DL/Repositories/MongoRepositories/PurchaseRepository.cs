using System.Linq.Expressions;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Configurations;
using BookStoreDK.Models.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreDK.DL.Repositories.MongoRepositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IMongoCollection<Purchase> _collection;

        public PurchaseRepository(IOptionsMonitor<MongoDbConfiguration> settings)
        {
            var client = new MongoClient(settings.CurrentValue.ConnectionString);
            var database = client.GetDatabase(settings.CurrentValue.DatabaseName);
            _collection = database.GetCollection<Purchase>(settings.CurrentValue.CollectionName);
        }

        public async Task<Guid> DeletePurchase(Purchase purchase)
        {
            Expression<Func<Purchase, bool>> deleteFilter = x => x.Id == purchase.Id;
            await _collection.DeleteOneAsync<Purchase>(deleteFilter);
            return purchase.Id;
        }

        public async Task<IEnumerable<Purchase>> GetPurchases(int userId)
        {                      
            return await (await _collection.FindAsync(x=>x.UserId == userId)).ToListAsync();
        }

        public async Task<Purchase?> SavePurchase(Purchase purchase)
        {
            await _collection.InsertOneAsync(purchase);
            return purchase;
        }
    }
}
