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
using System.Net;
using Infrastructures.Context.Data;

namespace Infrastructures.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<InvoiceGeneratorCollarCompanyContext> _userManager;
        private readonly IHttpContextAccessor _ContextAccesor;
        public CartRepository(ApplicationDbContext db, UserManager<InvoiceGeneratorCollarCompanyContext> userManager, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _ContextAccesor = contextAccessor;
        }
        public async Task<int> AddItemToCart(SizeProduct sizeproductId, int materialId, int quantity)
        {
            string userId = GetUsersId();

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Użytkownik jest niezalogowany");
            }
            var cart = await GetCart(userId);

            //sprawdzenie czy koszyk został wcześniej utworzony dla danego użytkownika
            if (cart is null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId
                };
                await _db.ShoppingCarts.AddAsync(cart);
            }
            await _db.SaveChangesAsync();

            SizeProduct sizeProduct = new SizeProduct();
            sizeProduct.Id = sizeproductId.Id;

            var cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.SizeProductId == sizeproductId.Id && a.MaterialId == materialId);
            if (cartItem is not null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var material = _db.Materials.Find(materialId);


                cartItem = new CartDetail
                {
                    SizeProductId = sizeproductId.Id,
                    MaterialId = materialId,
                    ShoppingCartId = cart.Id,
                    Quantity = quantity
                };
                await _db.CartDetails.AddAsync(cartItem);
            }
            await _db.SaveChangesAsync();


            //}
            //catch (Exception ex)
            //{
            //    transaction.Rollback();
            //}
            var cartItemCount = await GetItemCountCart(userId);
            return cartItemCount;
        }
        public async Task<int> RemoveItemWithCart(SizeProduct sizeproductId, int materialId, int sizeId)
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
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.SizeProductId == sizeproductId.Id && a.MaterialId == materialId); //nie działa prawidłowo do zmiany 
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

        public async Task<IEnumerable<CartDetail>> GetUserCart()
        {
            var userId = GetUsersId();

            if (userId == null)
            {
                throw new Exception("Invalid userId");

            }

            var selectedshoppingCart = await (from shoppingCart in _db.ShoppingCarts
                                              where shoppingCart.UserId == userId
                                              select new ShoppingCart
                                              {
                                                  Id = shoppingCart.Id,
                                                  UserId = shoppingCart.UserId,
                                                  CartDetails = shoppingCart.CartDetails,
                                                  IsDeleted = shoppingCart.IsDeleted,
                                              }).FirstAsync();

            int selectedShoppingCartId = selectedshoppingCart.Id;

            var cartDetails = await _db.CartDetails
            .Include(cd => cd.Materials)
            .Include(cd => cd.SizeProducts.Product)
            .Include(cd => cd.SizeProducts.Size)
            .Where(cd => cd.ShoppingCartId == selectedShoppingCartId)
            .ToListAsync();

            //var shoppingCart = await (from shoppingcart in _db.ShoppingCarts
            //                          join cartDetail in _db.CartDetails on shoppingcart.Id equals cartDetail.ShoppingCartId
            //                          where (shoppingcart.UserId == userId )
            //                          select new ShoppingCart()).FirstAsync();

            //var shoppingCart = await _db.ShoppingCarts
            //                        .Include(a => a.CartDetails)
            //                        .ThenInclude(a => a.SizeProducts)
            //                        .Include(a => a.CartDetails)
            //                        .ThenInclude(a => a.Materials)
            //                        .Where(a => a.UserId == userId).FirstOrDefaultAsync();


            return cartDetails;


        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var userCart = await _db.ShoppingCarts.FirstOrDefaultAsync(u => u.UserId == userId);
            return userCart;
        }
        //Zwraca liczbę itemów z koszyka
        public async Task<int> GetItemCountCart(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUsersId();
            }
            var data = await (from cart in _db.ShoppingCarts
                              join CartDetail in _db.CartDetails
                             on cart.Id equals CartDetail.ShoppingCartId
                              where cart.UserId == userId
                              select new { CartDetail.Id }).ToListAsync();

            return data.Count;
        }

        public async Task<bool> DoCheckout()
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUsersId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");

                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _db.CartDetails
                                    .Where(a => a.Id == cart.Id).ToList();

                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var pendingRecord = _db.OrderStatuses.FirstOrDefault(s => s.StatusName == "Pending");
                if (pendingRecord is null)
                    throw new InvalidOperationException("Order status does not have Pending status");
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1//pending
                };

                _db.Orders.Add(order);
                _db.SaveChanges();

                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {

                        CartDetailId = item.Id,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);
                
                
                }
            
                    //_db.SaveChanges();

                   // removing the cartdetails
                  _db.CartDetails.RemoveRange(cartDetail);
                  _db.SaveChanges();
                  transaction.Commit();
                  return true;
            }   
            catch (Exception ex)
            {

                return false;
            }
        }

        private string GetUsersId()
        {
            var principal = _ContextAccesor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
