using InvoiceGeneratorCollarCompany.Models;

namespace InvoiceGeneratorCollarCompany.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Product { get; set; }
        public IEnumerable<Type> Type { get; set; }
        public IEnumerable<Material> Material { get; set; }

        public string STerm { get; set; } = "";
        public int TypeId { get; set; } = 0;

        public int SizeId { get; set; } = 0;
        public int MaterialId { get; set; } = 0;
    }
}
