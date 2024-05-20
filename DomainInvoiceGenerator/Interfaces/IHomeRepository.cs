using Domain.Models;

namespace Domain.Interfaces
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Domain.Models.Type>> TypeLabel();
        Task<IEnumerable<Product>> GetProducts(string sTerm = "", int typeId = 0,int sizeId=0);
        Task<IEnumerable<Size>> GetSizes(Product product);
        Task<IEnumerable<Material>> GetMaterials();
        
        
    }
}