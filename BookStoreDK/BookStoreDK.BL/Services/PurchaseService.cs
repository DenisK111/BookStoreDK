using System.Net;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseService(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<PurchaseResponse> DeletePurchase(Guid purchaseId,int userId)
        {
            var purchase = (await _purchaseRepository.GetPurchases(userId)).SingleOrDefault(x => x.Id == purchaseId);

            if (purchase == null)
            {
                return new PurchaseResponse()
                {
                    HttpStatusCode =HttpStatusCode.BadRequest,
                    Message = "No such purchase exists"
                };
            }
            await _purchaseRepository.DeletePurchase(purchase);

            return new PurchaseResponse()
            {
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        public async Task<IEnumerable<Purchase>> GetPurchases(int userId)
        {
             return await _purchaseRepository.GetPurchases(userId);
                     
        }

        public async Task<Purchase?> SavePurchase(Purchase purchase)
        {
            return await _purchaseRepository.SavePurchase(purchase);
        }
    }
}
