using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CheckoutViewModel
    {
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<OrderDetailViewModel> OrderDetailsModel { get; set; }
        public decimal TotalAmount { get; set; }
        public UserViewModel User { get; set; }
    }

    public class OrderDetailViewModel
    {
        public Product Products { get; set; }
        public int Quantity { get; set; }
        public int SizeId { get; set; }
        public double UnitPrice { get; set; }
        public int SizeProductId { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public SizeProduct SizeProducts  { get; set; }
        public int MaterialId { get; set; }
        public Size Sizes { get; set; }
        public Material Materials { get; set; }
    }

    public class UserViewModel
    {

        public string Name { get; set; }

        public string Surname { get; set; }
        public string Email { get; set; }

        public string CompanyName { get; set; }

        public string Street { get; set; }

        public string NumberOf { get; set; }

        public string PostCode { get; set; }
    }
}
