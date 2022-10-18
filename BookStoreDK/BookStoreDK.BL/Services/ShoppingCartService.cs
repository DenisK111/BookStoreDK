using System.Net;
using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Services
{
    public class ShoppingCartService : IShoppingCartService
    {

        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartService(IPurchaseRepository purchaseRepository, IBookRepository bookRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _purchaseRepository = purchaseRepository;
            _bookRepository = bookRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<ShoppingCartResponse> GetPurchase(int userId)
        {
            var shoppingCartResponse = await _shoppingCartRepository.GetPurchase(userId);

            if (shoppingCartResponse == null)
            {
                return new ShoppingCartResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = "No Such User Found"
                };

            }

            return new ShoppingCartResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = shoppingCartResponse
            };

        }

        public async Task<ShoppingCartResponse> AddToCart(int bookId, int userId)
        {
            var book = await _bookRepository.GetById(bookId);

            if (book == null)
            {
                return new ShoppingCartResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "No such Book exists in the inventory"
                };
            }

            var result = await _shoppingCartRepository.AddToCart(book, userId);

            return new ShoppingCartResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = result
            };

        }

        public async Task<ShoppingCartResponse> RemoveFromCart(int bookId, int userId)
        {
            var book = await _bookRepository.GetById(bookId);

            if (book == null)
            {
                return new ShoppingCartResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "No such Book exists in the inventory"
                };
            }

            var result = await _shoppingCartRepository.RemoveFromCart(bookId, userId);

            if (result.cart == null)
            {
                return new ShoppingCartResponse()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Message = $"{result.deletedCount} books deleted, Cart is now empty."
                };
            }

            if (!result.cart.Books.Any())
            {
                return new ShoppingCartResponse()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Message = "No items found in Cart."
                };
            }

            return new ShoppingCartResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = result.cart
            };

        }

        public void EmptyCart(int userId)
        {
            _shoppingCartRepository.EmptyCart(userId);
        }

        public async Task<PurchaseResponse> FinishPurchase(int userId)
        {
            var shoppingCart = await _shoppingCartRepository.GetPurchase(userId);

            if (shoppingCart == null)
            {
                return new PurchaseResponse()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = "No items found in cart"
                };
            }

            await _shoppingCartRepository.EmptyCart(userId);

            var purchase = new Purchase()
            {
                Books = shoppingCart.Books,
                TotalMoney = shoppingCart.Books.Select(x => x.Price).Sum(),
                UserId = userId
            };

            var result = await _purchaseRepository.SavePurchase(purchase);

            return new PurchaseResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Model = result
            };
        }


    }
}
