using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Intefraces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> AddToCart(Book book, int userId);

        Task EmptyCart(int userId);

        Task<ShoppingCart> GetPurchase(int userId);

        Task<(ShoppingCart cart, long deletedCount)> RemoveFromCart(int bookId, int userId);
    }
}