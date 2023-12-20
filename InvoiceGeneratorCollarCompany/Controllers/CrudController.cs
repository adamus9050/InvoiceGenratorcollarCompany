using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Application.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace InvoiceGeneratorCollarCompany.Controllers
{
    public class CrudController : Controller
    {
        private readonly ILogger<CrudController> _logger;
        private readonly ICrudRepository _crudRepository;
        private readonly IHomeRepository _homeRepository;

        public CrudController(ILogger<CrudController> logger,IHomeRepository homeRepository, ICrudRepository crudrepository)
        {
            _logger = logger;
            _crudRepository = crudrepository;
            _homeRepository= homeRepository;
        
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct() 
        {    if(!User.Identity.IsAuthenticated || User.Identity== null)
            {
                return RedirectToPage("/Account/Login", new {area = "Identity"});
            }//to samo można zrobić za pomocą [Authorized]
            else
            {
                IEnumerable<Domain.Models.Type> types = await _homeRepository.TypeLabel();
                ProductDisplayModel type = new ProductDisplayModel
                {
                    Type = types,
                };
                    return View(type);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product) 
        {
                var productadd = await _crudRepository.AddProduct(product);
                
                TempData["Product"] = productadd;
                return RedirectToAction("AddSizeToProduct");
            //Dodać walidację produktu

        }

        [Authorize]
        [HttpGet]
        public IActionResult AddMaterial()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMaterial(Material material)
        {
            if(ModelState.IsValid)
            {
                var materialadd = await _crudRepository.AddMaterial(material);
    
                TempData["Product"] = materialadd;
                return RedirectToAction("AddMaterial");
            }
            else
            {
                return View();
            }

            //Dodać walidację materiału
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddSize()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSize(Size size)
        {
            var sizeadd = await _crudRepository.AddSizes(size);

            TempData["Product"] = sizeadd;
            return RedirectToAction("AddProduct");
            //dodać walidację rozmiaru
        }

       //dodać listowanie materiałów, produktów wraz z przypisanymi materiałami oraz rozmiarami, listowanie rozmiarów.


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddSizeToProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSizeToProduct(int product, int size)
        {

            var productSize = _crudRepository.AddSizeProduct(product, size);

            TempData["Product"] = productSize;
            return RedirectToAction("AddProduct");
        }

        //public async Task<IActionResult> Search(string prodName)
        //{
        //    if (_crudRepository.Search == null)
        //    {
        //        return Problem("Nie ma takiego produktu w bazie");
        //    }
        //    var produkt = _crudRepository.Search(prodName);
        //    return View(produkt);
        //}

    }
}
