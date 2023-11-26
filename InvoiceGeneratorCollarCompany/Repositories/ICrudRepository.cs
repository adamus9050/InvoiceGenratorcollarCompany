using InvoiceGeneratorCollarCompany.Models;

namespace InvoiceGeneratorCollarCompany.Repositories
{
    public interface ICrudRepository
    {
        public string AddProduct(Product product);
        public string AddMaterial(Material material);
        public Size AddSizes(Size size);
        //public Task AddSizeProduct(string NameProduct, string nameSize);
        public Task<List<Product>> Search(string prodName);

    }
}
