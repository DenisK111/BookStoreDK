using BookStoreDK.BL.Interfaces;
using BookStoreDK.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreDK.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IPurchaseService _purchaseService;
        private readonly IBookService _bookService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IPurchaseService purchaseService,IBookService bookService)
        {
            _shoppingCartService = shoppingCartService;
            _purchaseService = purchaseService;
            _bookService = bookService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetPurchase()
        {
            var userId = GetUserId();
            return this.ProduceResponse(await _shoppingCartService.GetPurchase(userId));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost(nameof(AddToCart))]
        public async Task<IActionResult> AddToCart(int bookId)
        {
            var userId = GetUserId();          

          
            return this.ProduceResponse(await _shoppingCartService.AddToCart(bookId, userId));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete(nameof(RemoveFromCart))]
        public async Task<IActionResult> RemoveFromCart(int bookId)
        {
            var userId = GetUserId();
            var result = await _shoppingCartService.RemoveFromCart(bookId, userId);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete(nameof(EmptyCart))]
        public IActionResult EmptyCart()
        {
            var userId = GetUserId();
            _shoppingCartService.EmptyCart(userId);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(nameof(FinishPurchase))]
        public async Task<IActionResult> FinishPurchase()
        {
            var userId = GetUserId();
            return this.ProduceResponse(await _shoppingCartService.FinishPurchase(userId));
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(nameof(GetPurchases))]
        public async Task<IActionResult> GetPurchases()
        {
            var user = GetUserId();
            return Ok(await _purchaseService.GetPurchases(user));
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete(nameof(DeletePurchase))]
        public async Task<IActionResult> DeletePurchase(Guid purchaseId)
        {
            var user = GetUserId();
            return this.ProduceResponse(await _purchaseService.DeletePurchase(purchaseId,user));
        }

        private int GetUserId()
        {
            return int.Parse(this.User.Claims.First(c => c.Type == "UserId").Value);
        }

    }
}
