using Infrastructures.Context;
using Domain.Models;
using Domain.Interfaces;
using Application.DTOs;
using Microsoft.VisualBasic;
using NuGet.Protocol;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
//using System.Drawing;

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
            await _db.SaveChangesAsync(); 
         
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
        public async Task<Product> GetProduct(int prodId)
        {
            var selctedProduct = _db.Products.Find(prodId);
            Product product = new Product();
            product.ProductId = prodId;
            product.ProductName=selctedProduct.ProductName;
            product.ProductPrice = selctedProduct.ProductPrice;
            product.Description = selctedProduct.Description;
            product.Image = selctedProduct.Image; 
            product.CreatedAt = selctedProduct.CreatedAt;

            return product;

        }
        public async Task<Size> GetSize(int sizeId)
        {
            var sizeSelected = await _db.Sizes.FindAsync(sizeId);
            Size size = new Size();
            size.Id = sizeSelected.Id;
            size.NameInt = sizeSelected.NameInt;
            size.Namestring = sizeSelected.Namestring;
   
            return size;
        }
        public async Task<IEnumerable<Material>> GetMaterialList()
        {
            var material = await _db.Materials.ToListAsync();
            return material;
        }
        public async Task<Material> GetMaterial(int materialId)
        {
            var material = await _db.Materials.FindAsync(materialId);
            return material;

        }

        public async Task<IEnumerable<Size>> GetSizeList()
        {
            var sizeList = _db.Sizes.ToList();
            return sizeList;
        }
        public async Task Delete(int prodId=0,int sizeId=0, int materialId=0)
        {
            if(prodId != 0)
            {
                var selctedProduct = _db.Products.Find(prodId);
                var selectedsize =  from size in _db.sizeProducts
                                          where size.ProductId == prodId
                                          select size; // Wybranie rozmiarów przypisanych do produktu

                _db.Products.Remove(selctedProduct);
                foreach(var item in selectedsize)
                {
                _db.sizeProducts.Remove(item); // usunięcie rozmiarów przypisanych do produktu

                }

            }
            if(sizeId != 0) 
            {
                var selctedSize = _db.Sizes.Find(sizeId);
                _db.Sizes.Remove(selctedSize);
            }
            if(materialId != 0) 
            {
                var selctedMaterial = _db.Materials.Find(materialId);
                _db.Materials.Remove(selctedMaterial);
            }

            _db.SaveChanges();
        }
        public async Task Edit(int prodId = 0, int sizeId = 0, int materialId = 0)
        {
            if (prodId != 0)
            {
                var selctedProduct = _db.Products.
            }
            if (sizeId != 0)
            {
                var selctedSize = _db.Sizes.Find(sizeId);
                _db.Sizes.Remove(selctedSize);
            }
            if (materialId != 0)
            {
                var selctedMaterial = _db.Materials.Find(materialId);
                _db.Materials.Remove(selctedMaterial);
            }

            _db.SaveChanges();
        }
        public async Task AddSizeProduct(int productId, int sizeId)
        {
            SizeProduct sizeproduct = new SizeProduct();
            sizeproduct.ProductId = productId;

            sizeproduct.SizeId = sizeId;


            _db.sizeProducts.Add(sizeproduct);
            await _db.SaveChangesAsync();
            //using (var transaction = _db.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        _db.sizeProducts.Add(sizeproduct);
            //        _db.SaveChanges();
            //        transaction.Commit();
            //    }
            //    catch (Exception)
            //    {
       
            //        transaction.Rollback();
            //    }
            //}
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
