using Microsoft.AspNetCore.Mvc;
using InvoiceGeneratorCollarCompany.Repositories;
using InvoiceGeneratorCollarCompany.Models;

namespace InvoiceGeneratorCollarCompany.Controllers
{
    public class CrudController : Controller
    {
        private readonly ILogger<CrudController> _logger;
        private readonly ICrudRepository _crudRepository;



        public CrudController(ILogger<CrudController> logger, ICrudRepository crudrepository)
        {
            _logger = logger;
            _crudRepository = crudrepository;
        }

        [HttpGet]
        public IActionResult AddProduct() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product) 
        {
            var productadd = _crudRepository.AddProduct(product);

            TempData["Product"] = productadd;
            return RedirectToAction("AddProduct");
        }

        [HttpGet]
        public IActionResult AddMaterial()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMaterial(Material material)
        {
            var materialadd = _crudRepository.AddMaterial(material);

            TempData["Product"] = materialadd;
            return RedirectToAction("Add");
        }

        [HttpGet]
        public IActionResult AddSize()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSize(Size size)
        {
            var sizeadd = _crudRepository.AddSizes(size);

            TempData["Product"] = sizeadd;
            return RedirectToAction("AddProduct");
        }

        [HttpGet]
        public IActionResult AddSizeToProduct()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> AddSizeToProduct(string product, string size)
        //{
        //    var productSize = _crudRepository.AddSizeProduct(product, size);

        //    TempData["Product"] = productSize;
        //    return RedirectToAction("AddProduct");
        //}
        
        public async Task<IActionResult> Search(string prodName)
        {
            if(_crudRepository.Search==null)
            {
                return Problem("Nie ma takiego produktu w bazie");
            }
            var produkt = _crudRepository.Search(prodName);
            return View(produkt);
        }

    }
}
