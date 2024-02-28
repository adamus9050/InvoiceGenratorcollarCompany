using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfces;
using Domain.Models;
using Infrastructures.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace Infrastructures.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _ContextAccesor;
        private readonly UserManager<IdentityUser> _userManager;


        public UserOrderRepository(ApplicationDbContext db, IHttpContextAccessor contextAccessor,UserManager<IdentityUser> userManager) 
        {
            _db = db;
            _ContextAccesor = contextAccessor;  
            _userManager= userManager;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUsersId();
            if(string.IsNullOrEmpty(userId)) 
            {
                throw new Exception("Użytkownik nie jest zalogowany");
            }
            var orders = await _db.Orders
                .Include(x =>x.OrderDetail)
                //.ThenInclude(x =>x.SizeProducts)
                .Where(a => a.UserId == userId)
                .ToListAsync();
            return orders;
        }
        private string GetUsersId()
        {
            var principal = _ContextAccesor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
