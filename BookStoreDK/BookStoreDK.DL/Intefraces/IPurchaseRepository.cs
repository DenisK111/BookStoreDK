using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Intefraces
{
    public interface IPurchaseRepository
    {
        Task<Purchase?> SavePurchase(Purchase purchase);
        
        Task<Guid> DeletePurchase (Purchase purchase);

        Task<IEnumerable<Purchase>> GetPurchases(int userId);
    }
}
