using Domain.Models;

namespace Domain.Interfaces
{
    public interface ICrudRepository
    {
        public Task<string> AddProduct(Product product);
        public Task<string> AddMaterial(Material material);
        public Task<Size> AddSizes(Size size);
        public Task AddSizeProduct(int NameProduct, int nameSize);
        public Task<Product> GetProduct(int prodId);
        public Task<Product> GetProductSizeProductId(int productSizeId);

        public Task<Size> GetSize(int sizeId);
        public Task<IEnumerable<Size>> GetSizeList();
        public Task<IEnumerable<Material>> GetMaterialList();
        public Task<Material> GetMaterial(int materialId);
        public Task<SizeProduct> GetSizeProduct(int productId, int sizeId);

        public Task Delete(int prodId, int sizeId,int materialId);

      
       //public Task<List<Product>> Search(string prodName);

    }
}
