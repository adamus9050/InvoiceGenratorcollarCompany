using Domain.Interfaces;
using Domain.Interfces;
using Domain.Models;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGeneratorCollarCompany.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICrudRepository _crudRepository;


        public CartController(ICartRepository cartRepository, ICrudRepository crudRepository)
        {
            _cartRepository = cartRepository;
            _crudRepository= crudRepository;
        }

        public async Task<IActionResult> AddItem(int productId, int materialId, int sizeId, int quantity = 1, int redirect = 0)
        {
            var sizeproduct = await _crudRepository.GetSizeProduct(productId,sizeId);
            var cartCount = await _cartRepository.AddItemToCart(sizeproduct,materialId, quantity);
            if (redirect == 0)
            {
                return Ok(cartCount);

            }
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int productId, int materialId)
        {
            var cartCount = await _cartRepository.RemoveItemWithCart(productId, materialId);

            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
           IEnumerable<CartDetail> userCart = await _cartRepository.GetUserCart(); 
            return View(userCart);
        }
        public async Task<IActionResult> GetTotalItemCart()
        {
            int ItemCart = await _cartRepository.GetItemCountCart();
            return Ok(ItemCart);
        }
        public async Task<IActionResult> Checkout()
        {
            bool isCheckedOut = await _cartRepository.DoCheckout();
            if (isCheckedOut)
            {
                throw new Exception("Coś poszło nie trak po stronie serwera");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
