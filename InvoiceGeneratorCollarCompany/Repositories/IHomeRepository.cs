using InvoiceGeneratorCollarCompany.Models;

namespace InvoiceGeneratorCollarCompany
{
    public interface IHomeRepository
    {
        Task<IEnumerable<InvoiceGeneratorCollarCompany.Models.Type>> TypeLabel();
        Task<IEnumerable<Product>> GetProducts(string sTerm = "", int typeId = 0,int sizeId=0);
        Task<IEnumerable<Size>> GetSizes(Product product);
        Task<IEnumerable<Material>> GetMaterials();
        
        
    }
}