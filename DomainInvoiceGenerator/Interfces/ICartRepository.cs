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
        public Task<int> AddItemToCart(int productId, int quantity,int sizeId, int materialId);
        public Task<int> RemoveItemWithCart(int productId, int materialId);
        public Task<ShoppingCart> GetUserCart();
        public Task<int> GetItemCountCart(string userId = "");
        public Task<ShoppingCart> GetCart(string userId);
    }
}
