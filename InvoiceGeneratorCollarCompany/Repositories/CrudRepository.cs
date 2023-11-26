using InvoiceGeneratorCollarCompany.Data;
using InvoiceGeneratorCollarCompany.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGeneratorCollarCompany.Repositories
{
    public class CrudRepository : ICrudRepository
    {
        private readonly ApplicationDbContext _db;
        public CrudRepository(ApplicationDbContext db)
        {
            _db=db;
        }


        public string AddProduct(Product product)
        {
             _db.Products.Add(product);
            if(_db.SaveChanges() > 0)
            {
                Console.WriteLine("Dodano produkt do bazy danych");
            }
                var productString = $"I.d:{product.ProductId} Nazwa:{product.ProductName} Opis:{product.Description} Cena:{product.ProductPrice}";

              return productString;    
        }
        public string AddMaterial(Material material)
        {
            _db.Materials.Add(material);
            if (_db.SaveChanges() > 0)
            {
                Console.WriteLine("Dodano material do bazy danych");
            }
                var materialString = $"I.d:{material.MaterialId} Nazwa:{material.Name} Opis:{material.Colour} Cena:{material.Price}";

            return materialString;
        }
        public Size AddSizes(Size size)
        {
            _db.Sizes.Add(size);
            if (_db.SaveChanges() > 0)
            {
                Console.WriteLine("Dodano rozmiar do bazy");//var SizesString = $"I.d:{size.Id} NazwaInt:{size.NameInt} NazwaStr:{size.Namestring}";
            }

            return size;
        }

        //public async Task AddSizeProduct(string nameProduct, string nameSize)
        //{
        //    //Search(nameProduct);
        //    var productstr = _db.Products.FindAsync(nameProduct);
        //    var sizestr = _db.Sizes.FindAsync(nameSize);

        //    SizeProduct sizeProduct =  new SizeProduct();
        //    sizeProduct.Product = productstr;
        //    sizeProduct.Size = sizestr;



        //    _db.sizeProducts.Add(sizeProduct);

        //    //return sizeProduct;
        //}

        public async Task<List<Product>> Search(string prodName)
        {
            var product = from m in _db.Products
                           select m;

            product = product.Where(x => x.ProductName!.Contains(prodName));
            var productid = await product.ToListAsync();

            return productid;
        }
    }
}
