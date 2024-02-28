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
        //Dodawanie porduktu do bazy 
        [HttpGet]
        public async Task<IActionResult> AddProduct() 
        {   //Autoryzacja sposób 1
            if (!User.Identity.IsAuthenticated || User.Identity == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }//to samo można zrobić za pomocą [Authorized]  
            if(!User.IsInRole("Admin"))
            {
                return RedirectToAction("NoAccessAuthorization","Home");
            }       
            else
            {
                IEnumerable<Domain.Models.Type> types = await _homeRepository.TypeLabel();
                ProductDisplayModel type = new ProductDisplayModel
                {
                    Type = types,
                };
                    return View("Product/AddProduct",type);
            }
            

        } // dodanie porduktu

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product) 
        {

                var productadd = await _crudRepository.AddProduct(product);
                
                TempData["Product"] = productadd;
            
            //Wszystkie nowo dodane produkty przechodzą do dodania rozmiaru.

                return RedirectToAction("AddSizeToProduct",product);
            //*******Dodać walidację produktu

        }
  
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>Delete(int prodId=0,int sizeId=0,int materialId=0)
        {
            if(prodId > 0)
            {
               await _crudRepository.Delete(prodId, sizeId, materialId);
               return RedirectToAction("Index","Home");
            }
            else if(sizeId >0)
            {
                await _crudRepository.Delete(prodId, sizeId, materialId);
                return RedirectToAction("Delete_Size");
            }
            else
            {
                await _crudRepository.Delete(prodId, sizeId, materialId);               
                return RedirectToAction("GetMaterialList");
            }
        }

        //Dodawanie materialu do bazy 
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddMaterial()
        {

            return View("Material/AddMaterial");
        } //dodawanie materiałów

        [HttpPost]
        public async Task<IActionResult> AddMaterial(Material material)
        {
            if(!ModelState.IsValid)
            {
                var materialadd = await _crudRepository.AddMaterial(material);
    
                TempData["Material"] = materialadd;
                return RedirectToAction("AddMaterial");
            }
            else
            {
                return View("Error");
            }

            //*********Dodać walidację materiału
        }

        [HttpGet]
        public async Task <IActionResult> GetMaterialList()
        {
            var material =await _crudRepository.GetMaterialList();
            return View("Material/GetMaterialList",material);

        } // lista materiałów

        //Dodawanie romzmiarów do bazy
        [Authorize(Roles = "Admin")]
        public IActionResult AddSize()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("NoAccessAuthorization", "Home");
            }
            return View("Size/AddSize");
        }

        [HttpPost]
        public async Task<IActionResult> AddSize(Size size)
        {
           
              var sizeadd = await _crudRepository.AddSizes(size);
              TempData["Size"] = $" NazwaStr:{sizeadd.Namestring}{"  "}NazwaInt:{sizeadd.NameInt}";
              return RedirectToAction("AddSize");            
            //dodać walidację rozmiaru
        }

        [HttpGet]
        public async Task<IActionResult> GetSizeList()
        {
            var sizeList =await _crudRepository.GetSizeList();
            return View("Size/GetSizeList",sizeList);

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSizeToProduct(int productId)
        {
            var product = await _crudRepository.GetProduct(productId);
            var prod = new Product
            {
                ProductId = productId,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                Description = product.Description,
                Image = product.Image
            };
            IEnumerable<Size> sizeList = await _crudRepository.GetSizeList();
            var prodSize = new ProductWithSizes
            {
                Id = productId,
                Name = product.ProductName,
                ProductPrice = product.ProductPrice,
                Description = product.Description,
                Image = product.Image,
                Sizes = sizeList                
            };
            return View(prodSize);
        }

        [HttpPost]
        public async Task<IActionResult> AddSizeToProduct(int productId, int[] selectedSize )
        {
            if(productId <= 0 ) // reakcja w przypadku błędnego Id produktu
            {
                return BadRequest("Id produktu jest nieprawidłowe. Mniejsze bądź równe 0");
            }
            else
            {
                foreach(var sizeId in selectedSize)
                {
                    await _crudRepository.AddSizeProduct(productId, sizeId);
                }
                
            }
            
            TempData["Product"] = productId;
            return RedirectToAction("Index","Home");
        }

    }
}
