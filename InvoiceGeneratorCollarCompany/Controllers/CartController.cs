using Application.DTOs;
using Domain.Interfaces;
using Domain.Models;
using Infrastructures.Context;
using Infrastructures.Context.Data;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace InvoiceGeneratorCollarCompany.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICrudRepository _crudRepository;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<InvoiceGeneratorCollarCompanyContext> _userManager;
        private readonly ApplicationDbContext _db;


        public CartController(ICartRepository cartRepository, ICrudRepository crudRepository, IEmailSender emailSender, ApplicationDbContext db, UserManager<InvoiceGeneratorCollarCompanyContext> userManager)
        {
            _cartRepository = cartRepository;
            _crudRepository = crudRepository;
            _emailSender = emailSender;
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddItem(int productId, int materialId, int sizeId, int quantity = 1, int redirect = 0)
        {
            if (sizeId == 0)
            {
                return RedirectToAction("ErrorAddSize", "Home"); //transfer walidation to contrroler home
            }
            else
            {
                var sizeproduct = await _crudRepository.GetSizeProduct(productId, sizeId);
                var cartCount = await _cartRepository.AddItemToCart(sizeproduct, materialId, quantity);

                if (redirect == 0)
                {
                    return RedirectToAction("GetUserCart", cartCount);
                }
            }

            return View("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int productId, int materialId, int sizeId)
        {
            var sizeproduct = await _crudRepository.GetSizeProduct(productId, sizeId);
            await _cartRepository.RemoveItemWithCart(sizeproduct, materialId, sizeId);

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

        [HttpGet]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            IEnumerable<CartDetail> cartDetails = await _cartRepository.GetUserCart();

            bool isCheckedOut = await _cartRepository.DoCheckout();

            var checkoutUserModel = new UserViewModel();
            if (!isCheckedOut)
            {
                throw new Exception("Coś poszło nie trak po stronie serwera");

            }
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                checkoutUserModel = new UserViewModel
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    CompanyName = user.CompanyName,
                    Street = user.Street,
                    NumberOf = user.NumberOf,
                    PostCode = user.PostCode

                };

                IEnumerable<OrderDetail> OrederdetailsModel = await _cartRepository.GetUserOrderDetail();

                List<OrderDetailViewModel> orders = new List<OrderDetailViewModel>();
                foreach (var item in OrederdetailsModel)
                {
                    var detailsModel = new OrderDetailViewModel
                    {
                        SizeProductId = item.SizeProductId,
                        SizeProducts = item.SizeProducts,
                        Sizes = await _crudRepository.GetSize(item.SizeProducts.SizeId),
                        Products = await _crudRepository.GetProductSizeProductId(item.SizeProductId),
                        MaterialId = item.MaterialId,
                        Materials = item.Materials,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    orders.Add(detailsModel);

                };

                CheckoutViewModel checkoutViewModel = new CheckoutViewModel
                {
                    User = checkoutUserModel,
                    OrderDetailsModel = orders,
                    ErrorMessage = "Koszyk został dodany"

                };

                string nazwaFaktury = $"Faktura{checkoutUserModel.Name}.xls";
                string plik = $@"D:\Programowanie\.Net\InvoiceGenratorcollarCompany\Faktury\{nazwaFaktury}";
                await _emailSender.ExcellDocGenerator($@"D:\Programowanie\.Net\InvoiceGenratorcollarCompany\Faktury\{nazwaFaktury}", cartDetails);
                await _emailSender.CreateTestMessage4(checkoutUserModel.Email, plik, "smtp.gmail.com");
                
                return View("Checkout", checkoutViewModel);
            }
            else
            {
                TempData["Błąd"] = "Coś poszło nie tak spróbuj jeszcze raz bądz skonatktuj się z adminem.";
                return View("Checkout");
            }

        }
        public async Task<IActionResult> InvoiceGenerate()
        {

            return View();
        }

    }
}
