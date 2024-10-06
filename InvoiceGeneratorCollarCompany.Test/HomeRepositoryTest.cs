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
    public class HomeRepositoryTest
    {
        //tworzenie bazy danych w pamiêci komputera
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // tworzenie nowej, unikalnej bazy dla ka¿dego testu
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task TypeLabel_SouldReturnAllEnableTypes()
        {

            // Arrange
            var context = GetInMemoryDbContext();

            context.Types.AddRange(new List<Domain.Models.Type>
            {
            new Domain.Models.Type { Id = 1, TypeName = "Type 1" },
            new Domain.Models.Type { Id = 2, TypeName = "Type 2" }
            });
            await context.SaveChangesAsync();

            var repository = new HomeRepository(context);

            // Act
            var result = await repository.TypeLabel();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Type 1", result.First().TypeName);
        }

        [Fact]
        public async Task GetProducts_ShouldGetAllProducts()
        {
            //Arrange
            var context = GetInMemoryDbContext();
            var type1 = new Domain.Models.Type { Id = 1, TypeName = "Typ 1" };
            var type2 = new Domain.Models.Type { Id = 2, TypeName = "Typ 2" };


            var prod1 = new Product { ProductId = 1, ProductName = "Ko³nierz1", CreatedAt = DateTime.Now, Description = "Jakiœ opis", 
                Image = " ", ProductPrice = 0.12, TypeId = 1 };
            var prod2 = new Product { ProductId = 2, ProductName = "Ko³nierz2", CreatedAt = DateTime.Now, Description = "Jakiœ opis2",
                Image = " ", ProductPrice = 0.14, TypeId = 1 };
            var prod3 = new Product { ProductId = 3, ProductName = "Ko³nierz3", CreatedAt = DateTime.Now, Description = "Jakiœ opis3",
                Image = " ", ProductPrice = 0.17, TypeId = 2 };

            context.AddRange(type1, type2);
            context.AddRange(new[] { prod1, prod2, prod3 });
            await context.SaveChangesAsync();

            var repository = new HomeRepository(context);

            //Act
            var result = await repository.GetProducts();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal(1, result.First().ProductId);
        }

        [Fact]
        public async Task GetSizes_ShouldReturnAllSizesForProducts()
        {
            //Arrange
            var context = GetInMemoryDbContext();

            var product = new Product { ProductId = 1, ProductName = "Ko³nierzyk1", CreatedAt = DateTime.Now, Description = "Jakiœ opis", Image = " ", ProductPrice = 0.12, TypeId = 1, TypeName = "Type1" };
            var size1 = new Size { Id = 1, NameInt = 32, Namestring = "s" };
            var size2 = new Size { Id = 2, NameInt = 33, Namestring = "s" };
            var size3 = new Size { Id = 3, NameInt = 34, Namestring = "m" };
            var size4 = new Size { Id = 4, NameInt = 35, Namestring = "l" };

            context.Products.Add(product);
            context.Sizes.AddRange(size1, size2, size3, size4);
            await context.SaveChangesAsync();

            context.sizeProducts.AddRange(
                new SizeProduct { ProductId = product.ProductId, SizeId = size1.Id },
                new SizeProduct { ProductId = product.ProductId, SizeId = size2.Id },
                new SizeProduct { ProductId = product.ProductId, SizeId = size3.Id },
                new SizeProduct { ProductId = product.ProductId, SizeId = size4.Id }

            );
            await context.SaveChangesAsync();


            var repository = new HomeRepository(context);

            //Act
            var result = await repository.GetSizes(product);

            //sprawdzenie czy dane siê prawid³owo dodaj¹ do bazy
            var resultList = result.ToList();
            foreach (var size in resultList)
            {
                Console.WriteLine($"Id: {size.Id}, NameInt: {size.NameInt}, Namestring: {size.Namestring}");
            }

            //Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
            Assert.Contains(result, s => s.Namestring == "s");
            Assert.Contains(result, s => s.Namestring == "m");
            Assert.Contains(result, s => s.Namestring == "l");
        }

        [Fact]
        public async Task GetMaterials_ShouldReturnAllMaterials()
        {
            //Arrange
            var context = GetInMemoryDbContext();

            context.Materials.AddRange(new List<Material>
            {
                new Material {MaterialId = 1, Colour = "Niebieski", Description= "Jakiœ opis", ImageColour=" ", Name = "Materia³1", Price=0.45 },
                new Material {MaterialId = 2, Colour = "Granat", Description= "Jakiœ opis2", ImageColour=" ", Name = "Materia³2", Price=0.4 },
            });
            context.SaveChangesAsync();
            var repository = new HomeRepository(context);

            //Act
            var result = await repository.GetMaterials();

            //Asserts
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, s => s.Name == "Materia³1");
            Assert.Contains(result, s => s.Name == "Materia³2");

        }

    }
}