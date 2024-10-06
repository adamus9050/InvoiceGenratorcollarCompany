using Xunit;
using Moq;
using System.Collections.Generic;
using Infrastructures.Repositories;
using Infrastructures.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace InvoiceGeneratorCollarCompany.Test
{
    public class CrudRepositoryTest
    {
        //tworzenie bazy danych w pamięci komputera
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // tworzenie nowej, unikalnej bazy dla każdego testu
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task  AddProduct_ShouldAddProductAndReturnCorrectString()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var repository = new CrudRepository(context);

            var testProduct = new Product
            {
                ProductName = "Nazwa Produktu 1",
                CreatedAt = DateTime.UtcNow,
                Description = "Description1",
                Image = " ",
                ProductPrice = 0.7,
                TypeId = 1,
                TypeName = "TypTestowy"

            };

            //Act
            var result = await repository.AddProduct(testProduct);

            //Assert
            var addedProduct = await context.Products.FindAsync(testProduct.ProductId);


        }
    }
}
