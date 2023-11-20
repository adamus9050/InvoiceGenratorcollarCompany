using InvoiceGeneratorCollarCompany.Models;

namespace InvoiceGeneratorCollarCompany.Repositories
{
    public interface ICrudRepository
    {
        public string AddProduct(Product product);
        public string AddMaterial(Material material);
        public Size AddSizes(Size size);
    }
}
