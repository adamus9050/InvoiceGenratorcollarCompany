using Application.DTOs;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceGeneratorCollarCompany.Controllers;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator.UnitTest.Controller
{
    public class HomeControllerTest
    {
        [Fact]
        public void _GetAll_GetAllProducts_ReturnsAllProductsWithSizes()
        {
            //var mockType = new Domain.Models.Type[]
            //{
            //    new(){Id=1, TypeName="TypKolnierz"},
            //    new(){Id=2, TypeName="TypInny"}
            //};

            //var mockSize = new Size[]
            //{
            //    new(){Id=1, NameInt=28, Namestring="xxs"},
            //    new(){Id=2,NameInt=29,Namestring="xxs"},
            //    new(){Id=3,NameInt=30,Namestring="xs"},
            //    new(){Id=4,NameInt=31,Namestring="xs"},
            //};

            //var mockProductWithSize = new SizeProduct[]
            //{
            //    new(){Id=1,ProductId=1,SizeId=1},
            //    new(){Id=2,ProductId=1,SizeId=2},
            //    new(){Id=3,ProductId=2,SizeId=3},
            //    new(){Id=4,ProductId=2,SizeId=4}
            //};

            //Arrange
            var mockProducts = new Product[]
            {
                new(){ProductId = 1, ProductName = "Product1", Description = "Description", CreatedAt=DateTime.Now, ProductPrice=1.23, TypeId=1},
                new(){ProductId = 2, ProductName = "Product2", Description = "Description2", CreatedAt=DateTime.Now, ProductPrice=0.65, TypeId=1},
                new(){ProductId = 2, ProductName = "Product3", Description = "Description3", CreatedAt=DateTime.Now, ProductPrice=3.66, TypeId=3}

            }.AsEnumerable();


            var mockRepo = new Mock<IHomeRepository>();
            mockRepo.Setup(r => r.GetProducts("", 0, 0)).ReturnsAsync(mockProducts);
            //mockRepo.Setup(x => x.GetProducts(" ",1,0).Result);

            var getProducts = new HomeController(null,mockRepo.Object);
            //Act
            var result = getProducts.Index().Result as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Collection( (result.Value as IEnumerable<Product>),
            r =>
            {
                Assert.Equal(1, r.ProductId);
                Assert.Equal("Product1", r.ProductName);
                Assert.Equal("Description", r.Description);
                Assert.Equal(DateTime.Now, r.CreatedAt);
                Assert.Equal(1.23, r.ProductPrice);
                Assert.Equal(1, r.TypeId);
            },
            r =>
            {
                Assert.Equal(1, r.ProductId);
                Assert.Equal("Product1", r.ProductName);
                Assert.Equal("Description", r.Description);
                Assert.Equal(DateTime.Now, r.CreatedAt);
                Assert.Equal(1.23, r.ProductPrice);
                Assert.Equal(1, r.TypeId);
            }
                );
        }

    }
}
