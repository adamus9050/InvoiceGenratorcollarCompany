using Domain.Interfces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace InvoiceGeneratorCollarCompany.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController( ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<IActionResult> AddItem(int ProductId,int materialId, int quantity=1,int redirect=0) 
        {

            var cartCount = await _cartRepository.AddItemToCart(ProductId,materialId,quantity);
            if(redirect == 0)
            {
                return Ok(cartCount);
            }
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int ProductId, int materialId) 
        {
            var cartCount = await _cartRepository.RemoveItemWithCart(ProductId, materialId);

            return RedirectToAction("GetUserCart"); 
        }
        public async Task<IActionResult> GetUserCart() 
        {
            var userCart = await _cartRepository.GetUserCart();
            return View(userCart);
        }
        public async Task<IActionResult> GetTotalItemCart() 
        {
            int ItemCart =await _cartRepository.GetItemCart();
            return Ok(ItemCart);
        }

    }
}
