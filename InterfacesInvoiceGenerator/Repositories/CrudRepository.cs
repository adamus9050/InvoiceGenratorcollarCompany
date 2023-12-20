using Infrastructures.Context;
using Domain.Models;
using Domain.Interfaces;
using Application.DTOs;


namespace Infrastructures.Repositories
{
    public class CrudRepository : ICrudRepository
    {
        private readonly ApplicationDbContext _db;
        public CrudRepository(ApplicationDbContext db)
        {
            _db=db;
        }


        public async Task<string> AddProduct(Product product)
        {
             await _db.Products.AddAsync(product);
            _db.SaveChangesAsync(); 
         
                var productString = $"I.d:{product.ProductId} Nazwa:{product.ProductName} Opis:{product.Description} Cena:{product.ProductPrice}";

              return productString;    
        }
        public async Task<string> AddMaterial(Material material)
        {
            await _db.Materials.AddAsync(material);
            _db.SaveChangesAsync();

                var materialString = $"I.d:{material.MaterialId} Nazwa:{material.Name} Opis:{material.Colour} Cena:{material.Price}";

            return materialString;
        }
        public async Task<Size> AddSizes(Size size)
        {
            await _db.Sizes.AddAsync(size);
            _db.SaveChangesAsync();

            return size;
        }

        public async Task AddSizeProduct(int nameProduct ,int nameSize)
        {
            var productstr = await _db.Products.FindAsync(nameProduct);


            //var sizestr = _db.Sizes.FindAsync(nameSize);

            //SizeProduct sizeProduct = new SizeProduct();
            //sizeProduct.Product = await productstr;
            //sizeProduct.Size = await sizestr;

            //_db.sizeProducts.Add(sizeProduct);

            //return productstr;

        }



        //public async Task<List<Product>> Search(string prodName)
        //{
        //    var product = from m in _db.Products
        //                   select m;

        //    product = product.Where(x => x.ProductName!.Contains(prodName));
        //    var productid = await product.ToListAsync();

        //    return productid;
        //}
    }
}
