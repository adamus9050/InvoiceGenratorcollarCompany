using Application.DTOs;
using Domain.Interfaces;
using Domain.Models;
using InvoiceGeneratorCollarCompany.Controllers;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace InvoiceGeneratorCollarCompany.Test
{
    public class HomeControllerTest
    {
        private readonly Mock<IHomeRepository> _mockRepository;
        private readonly HomeController _controller;

        public HomeControllerTest()
        {
            // Mockowanie repozytorium
            _mockRepository = new Mock<IHomeRepository>();

            // Mockowanie loggera
            var mockLogger = new Mock<ILogger<HomeController>>();

            // Tworzenie instancji kontrolera z użyciem zmockowanego repozytorium
            _controller = new HomeController(mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithProductDisplayModel()
        {
            // Arrange
            string sterm = "test";
            int typeId = 1;
            int sizeId = 1;

            var types = new List<Domain.Models.Type>
            {
                new Domain.Models.Type { Id = 1, TypeName = "NameOfType1" }
            };

            // Symulowane dane, które zwróci repozytorium
            var products = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    ProductName = "Product1",
                    Description = "Description1",
                    ProductPrice = 100,
                    Image = "image1.jpg",
                    TypeId = 1 
                }
            };

            var sizes = new List<Size>
            {
                new Size { Id = 1, NameInt = 32, Namestring="NazwaString" }
            };

            var materials = new List<Material>
            {
                new Material { MaterialId = 1, Name = "NazwaMaterial", Description= "OpisMateriału" }
            };



            // Mockowanie wywołań repozytorium
            _mockRepository.Setup(repo => repo.GetProducts(sterm, typeId, sizeId))
                .ReturnsAsync(products);

            _mockRepository.Setup(repo => repo.GetSizes(It.IsAny<Product>()))
                .ReturnsAsync(sizes);

            _mockRepository.Setup(repo => repo.GetMaterials())
                .ReturnsAsync(materials);

            _mockRepository.Setup(repo => repo.TypeLabel())
                .ReturnsAsync(types);

            // Act
            var result = await _controller.Index(sterm, typeId, sizeId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); 
            var model = Assert.IsType<ProductDisplayModel>(viewResult.Model); 

            // Sprawdzanie danych w modelu
            Assert.Single(model.ProductSize);
            Assert.Equal("Product1", model.ProductSize.First().Name);
            Assert.Equal("NameOfType1", model.Product.Type.TypeName);
            Assert.Single(model.Material);
            Assert.Single(model.Type);
            Assert.Equal(sterm, model.STerm);
            Assert.Equal(typeId, model.TypeId);
            Assert.Equal(sizeId, model.SizeId);
        }
    }
}
