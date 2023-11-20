using InvoiceGeneratorCollarCompany.Models.DTOs;
using InvoiceGeneratorCollarCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCore;

namespace InvoiceGeneratorCollarCompany.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homerepository;



        public HomeController(ILogger<HomeController> logger, IHomeRepository homerepository)
        {
            _logger = logger;
            _homerepository = homerepository;
        }

        public async Task<IActionResult> Index(string sterm = "", int typeId = 0, int sizeId = 0)
        {
            var products = await _homerepository.GetProducts(sterm, typeId, sizeId);
            List<ProductWithSizes> productWithSizes = new List<ProductWithSizes>();
            foreach (var prod in products)
            {
                var size = await _homerepository.GetSizes(prod);

                var productWithSize = new ProductWithSizes
                {
                    Id = prod.ProductId,
                    Name = prod.ProductName,
                    Description = prod.Description,
                    ProductPrice = prod.ProductPrice,
                    Image = prod.Image,
                    Sizes = size,

                };
                productWithSizes.Add(productWithSize);

                
                //prod.Materials = materials;
            }
            var materials = await _homerepository.GetMaterials();
            IEnumerable<InvoiceGeneratorCollarCompany.Models.Type> types = await _homerepository.TypeLabel();

            ProductDisplayModel productDisplay = new ProductDisplayModel
            {
                ProductSize = productWithSizes,
                Material = materials,
                Type = types,
                STerm = sterm,
                TypeId = typeId,
                SizeId = sizeId,

            };

            return View(productDisplay);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
