using BookStoreDK.Models.Models;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Interfaces
{
    public interface IPurchaseService
    {
        Task<Purchase?> SavePurchase(Purchase purchase);

        Task<PurchaseResponse> DeletePurchase(Guid purchaseId,int userId);

        Task<IEnumerable<Purchase>> GetPurchases(int userId);
    }
}
