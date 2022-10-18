using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartResponse> AddToCart(int bookId, int userId);
        void EmptyCart(int userId);
        Task<PurchaseResponse> FinishPurchase(int userId);
        Task<ShoppingCartResponse> GetPurchase(int userId);
        Task<ShoppingCartResponse> RemoveFromCart(int bookId, int userId);
    }
}