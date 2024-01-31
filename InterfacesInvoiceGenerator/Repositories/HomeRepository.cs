using Infrastructures.Context;
using Domain.Interfaces;
using Domain.Models;
using Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;
        public HomeRepository(ApplicationDbContext dbcontext)
        {
            _db = dbcontext;
        }
        public async Task<IEnumerable<Domain.Models.Type>> TypeLabel()
        {
            
            return await _db.Types.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts(string sTerm="", int typeId=0,int size=0) 
        {


            IEnumerable<Product> products = await (from product in _db.Products 
                            join type in _db.Types on product.TypeId equals type.Id
                            where string.IsNullOrWhiteSpace(sTerm) || (product!=null && product.ProductName.StartsWith(sTerm)) //Wyszukiwarka produktów
                            select new Product
                            {
                                ProductId = product.ProductId,
                                Image = product.Image,
                                ProductName = product.ProductName,
                                Description = product.Description,
                                ProductPrice = product.ProductPrice,
                                Type = product.Type,
                                TypeName= product.TypeName,
                            }).ToListAsync();
            if(typeId> 0)
            {
                products = products.Where(a =>a.TypeId == typeId).ToList();
            }
            return products;
        }

        public async Task<IEnumerable<Size>> GetSizes(Product product)
        {
            var sizes = await (from size in _db.sizeProducts
                               where product.ProductId == size.ProductId
                               select new Size
                               {
                                 Id=size.SizeId,
                                 NameInt=size.Size.NameInt
                               }).ToListAsync();
            return sizes;
           
        }

        public async Task<IEnumerable<Material>> GetMaterials()
        {
            var materials = await (from material in _db.Materials
                                   select new Material
                                   {
                                       MaterialId = material.MaterialId,
                                       Description = material.Description,
                                       Colour= material.Colour,
                                       ImageColour= material.ImageColour,
                                       Name = material.Name,
                                       Price = material.Price,
                                   }).ToListAsync();
            return materials;
        }

        //List product withoutrozmiars
        public async Task<IEnumerable<Product>> GetProductWithoutRozmiar()
        {
            var productWithoutSize = await (from product in _db.Products
                                   select new Product
                                   {
                                       ProductId = product.ProductId,
                                       Image = product.Image,
                                       ProductName = product.ProductName,
                                       Description = product.Description,
                                       ProductPrice = product.ProductPrice,
                                       Type = product.Type,
                                       TypeName = product.TypeName,
                                   }).ToListAsync();
            return productWithoutSize;
        }


    }
}
