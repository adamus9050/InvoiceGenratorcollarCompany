using Domain.Interfaces;
using Domain.Interfces;
using Domain.Models;
using Infrastructures.Context;
using Infrastructures.Context.Data;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace InvoiceGeneratorCollarCompany.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICrudRepository _crudRepository;
        private readonly ApplicationDbContext _db;


        public CartController(ICartRepository cartRepository, ICrudRepository crudRepository,ApplicationDbContext db)
        {
            _cartRepository = cartRepository;
            _crudRepository= crudRepository;
            _db = db;
        }

        public async Task<IActionResult> AddItem(int productId, int materialId, int sizeId, int quantity = 1, int redirect = 0)
        {
            if (sizeId == 0)
            {
                return RedirectToAction("ErrorAddSize", "Home"); //przeniesienie waldacji do kontrolera home

            }
            else
            {
               var sizeproduct = await _crudRepository.GetSizeProduct(productId,sizeId);
               
               var cartCount = await _cartRepository.AddItemToCart(sizeproduct,materialId, quantity);
               if (redirect == 0)
               {
               
                   return RedirectToAction("GetUserCart",cartCount);
               
               }
            }

            return View("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int productId, int materialId, int sizeId)
        {
            var sizeproduct = await _crudRepository.GetSizeProduct(productId, sizeId);
            await _cartRepository.RemoveItemWithCart(sizeproduct, materialId,sizeId);

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
            return Ok( ItemCart);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            bool isCheckedOut = await _cartRepository.DoCheckout();
            if (isCheckedOut)
            {
                throw new Exception("Coś poszło nie trak po stronie serwera");
            }

            return View("Checkout", "Cart");
        }
    }
}
