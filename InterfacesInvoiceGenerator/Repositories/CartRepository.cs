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
using Domain.Interfaces;

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

// Dodawanie produktów(itemów) do koszyka
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

            //Wybranie konkretnych danych produktu do koszyka 
            SizeProduct sizeProduct = new SizeProduct();
            sizeProduct.Id = sizeproductId.Id;
            var productPrice = await (from pr in _db.sizeProducts where sizeproductId.Id == pr.Id
                                 select new Product {ProductPrice = pr.Product.ProductPrice,ProductName=pr.Product.ProductName }).FirstAsync();
                
            // stworzenie koszyka z odpowiednimi danymi
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
                    UnitPrice = (productPrice.ProductPrice + (material.Price/10))*quantity,
                    ShoppingCartId = cart.Id,
                    Quantity = quantity
                };
                await _db.CartDetails.AddAsync(cartItem);
            }
            await _db.SaveChangesAsync();

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

//Pobieranie koszyka klienta 
        public async Task<IEnumerable<CartDetail>> GetUserCart()
        {
        //sprawdzenie Id klienta
            var userId = GetUsersId();

            if (userId == null)
            {
                throw new Exception("Invalid userId");

            }

        //znalezienie koszyka odpowiadającego dla danego klienta
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
            
        //Wyświetlenie koszyka, odpowiedniego dla konkretnego klienta
            var cartDetails = await _db.CartDetails
            .Include(cd => cd.Materials)
            .Include(cd => cd.SizeProducts.Product)
            .Include(cd => cd.SizeProducts.Size)
            .Where(cd => cd.ShoppingCartId == selectedShoppingCartId)
            .ToListAsync();


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
                
                // przesunięcie z CartDetail do OrderDetiail a następnie usunięcie koszyka z CartDetail
                var userId = GetUsersId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Użytkownik nie jest zalogowany");

                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Nieprawidłowy koszyk");

                var cartDetail = _db.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();

                if (cartDetail.Count == 0)
                    throw new Exception("Koszyk jest pusty");

                var pendingRecord = _db.OrderStatuses.FirstOrDefault(s => s.StatusName == "Przygotowywany");
                if (pendingRecord is null)
                    throw new InvalidOperationException("Order status does not have Pending status");

                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = pendingRecord.Id
                };

                _db.Orders.Add(order);
                _db.SaveChanges();

                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        SizeProductId = item.SizeProductId,
                        MaterialId = item.MaterialId,
                        SizeProducts = item.SizeProducts,
                        Materials = item.Materials,
                        Order = order,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    await _db.OrderDetails.AddAsync(orderDetail);                    

                }
                   await _db.SaveChangesAsync();                 

                   // Usunięcie CartDetails
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

        public async Task<IEnumerable<OrderDetail>> GetUserOrderDetail()
        {


            var selectedOrderDetails = await (from order in _db.Orders
                                              join detail in _db.OrderDetails on order.Id equals detail.OrderId
                                              join sizeproduct in _db.sizeProducts on detail.SizeProductId equals sizeproduct.Id
                                              where order.Id == detail.OrderId
                                              select new OrderDetail
                                              {
                                                  Quantity = detail.Quantity,
                                                  UnitPrice = detail.UnitPrice,
                                                  MaterialId = detail.MaterialId,
                                                  Order = order,
                                                  OrderId = order.Id,
                                                  Materials = detail.Materials,
                                                  SizeProductId = sizeproduct.Id,
                                                  SizeProducts = sizeproduct,
                                              }).ToListAsync();


            return selectedOrderDetails;


        }

        private string GetUsersId()
        {
            var principal = _ContextAccesor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

    }
}
