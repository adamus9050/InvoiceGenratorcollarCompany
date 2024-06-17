using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfces
{
    public interface ICartRepository
    {
        public Task<int> AddItemToCart(SizeProduct sizeproductId,int materialId, int quantity);
        public Task<int> RemoveItemWithCart(SizeProduct productId, int materialId,int sizeId);
        public Task<IEnumerable<CartDetail>> GetUserCart();
        public Task<IEnumerable<OrderDetail>> GetUserOrderDetail();
        public Task<int> GetItemCountCart(string userId = "");
        public Task<ShoppingCart> GetCart(string userId);
        public Task<bool> DoCheckout();
    }
}
