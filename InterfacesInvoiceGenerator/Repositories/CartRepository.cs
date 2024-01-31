using Domain.Models;
using Infrastructures.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Domain.Interfces;
using Application.DTOs;
using Microsoft.IdentityModel.Tokens;
//using System.Data.Entity;

namespace Infrastructures.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _ContextAccesor;
        public CartRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _ContextAccesor = contextAccessor;
        }
        public async Task<int> AddItemToCart(int productId,int materialId, int sizeId, int quantity)
        {
            string userId = GetUsersId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("Użytkownik jest niezalogowany");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId,
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();
                //cart details section
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCart_Id == cart.Id && a.ProductId == productId && a.MaterialId == materialId && a.SizeId==sizeId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += quantity;
                }
                else
                {
                    cartItem = new CartDetail
                    {
                        ProductId = productId,
                        MaterialId = materialId,
                        ShoppingCart_Id = cart.Id,
                        Quantity = quantity
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
               
            }
            catch (Exception ex)
            {
                
            }
            var cartItemCount = await GetItemCountCart(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItemWithCart(int productId, int materialId)
        {
            string userId = GetUsersId();
            try
            {
                
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("Użytkownik jest niezalogowany");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("Koszyk jest pusty");
                }
                _db.SaveChanges();
                //cart details section
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCart_Id == cart.Id && a.ProductId == productId && a.MaterialId == materialId);
                if (cartItem is null)
                {
                    throw new Exception("Brak produktów w koszyku");
                }
                else if (cartItem.Quantity == 1)
                {
                    _db.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = cartItem.Quantity - 1;
                }
                _db.SaveChanges();

                
            }
            catch (Exception ex)
            {
                
            }
            var cartItemCount = await GetItemCountCart(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUsersId();
            if(userId==null)
            {
                throw new Exception("Invalid userId");
                
            }
            var shoppingCart = _db.ShoppingCarts.Include(a => a.CartDetails)
                .ThenInclude(a => a.Products)    
                .Where(a => a.UserId == userId).FirstOrDefault();// tu zmiana
            return shoppingCart;
                
        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var userCart = await _db.ShoppingCarts.FirstOrDefaultAsync(u => u.UserId == userId);
            return userCart;
        }
        //Zwraca liczbę itemów z koszyka
        public async Task<int> GetItemCountCart(string userId="")
        {
            if(string.IsNullOrEmpty(userId)) 
            {
                userId= GetUsersId();
            }
            var data = await (from cart in _db.ShoppingCarts
                              join CartDetail in _db.CartDetails
                             on cart.CartId equals CartDetail.ShoppingCart_Id
                              select new { CartDetail.Id }).ToListAsync();
             return data.Count;
        }
        private string GetUsersId()
        {
            var principal = _ContextAccesor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
