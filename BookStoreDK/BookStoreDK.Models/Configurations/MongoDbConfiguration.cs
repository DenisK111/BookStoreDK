namespace BookStoreDK.Models.Configurations
{
    public class MongoDbConfiguration
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;

        public string ShoppingCartCollectionName { get; set; } = null!;
    }

}
