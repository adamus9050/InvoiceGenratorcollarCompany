using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using FluentAssertions;
using NUnit.Framework;
using Infrastructures.Context;
using Domain.Models;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;



namespace InvoiceGenerator.IntegrationTest
{
    public class CrudRepositoryTest
    {
        [Test, Isolated]
        public async Task AddProduct_PassValid_AddProductToDatabase() 
        {
            var context = new ApplicationDbContext();
            var product = new Product { ProductId = 1, ProductName = "ProductName", Description = "Description", ProductPrice = 0.56 };
            var repository = new CrudRepository(context);
            repository.AddProduct(product);

            var result = context.Products.Count(x =>x.ProductName == product.ProductName);
            Assert.That(result, Is.EqualTo(1));
        }

    }
}
