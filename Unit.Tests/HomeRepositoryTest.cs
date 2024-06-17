using Moq;
using Infrastructures.Context;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Unit.Tests
{
    public class HomeRepositoryTest
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly HomeRepository _repository;


        public HomeRepositoryTest()
        {
                _mockContext = new Mock<ApplicationDbContext>();
                _repository = new HomeRepository(_mockContext.Object);
        }

        [Fact]
        public void TypeLabel_ReturnsAllChoseTypes()
        {
            //Arrange
            var mockTypes = new List<Domain.Models.Type>
            {
                new Domain.Models.Type {Id = 1,TypeName = "Type1"},
                new Domain.Models.Type {Id = 2,TypeName = "Type2"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Domain.Models.Type>>();
        }
    }
}