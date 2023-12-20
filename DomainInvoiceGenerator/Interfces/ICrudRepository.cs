using Domain.Models;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ICrudRepository
    {
        public Task<string> AddProduct(Product product);
        public Task<string> AddMaterial(Material material);
        public Task<Size> AddSizes(Size size);
        public Task AddSizeProduct(int NameProduct, int nameSize);
       //public Task<List<Product>> Search(string prodName);

    }
}
