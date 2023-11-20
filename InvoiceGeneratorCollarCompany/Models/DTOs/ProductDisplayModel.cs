using InvoiceGeneratorCollarCompany.Models;
using System.ComponentModel.DataAnnotations;

namespace InvoiceGeneratorCollarCompany.Models.DTOs
{
    public class ProductWithSizes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double ProductPrice { get; set; }
        public string? Image { get; set; }
        public int  TypeId { get; set; }
        public Type Type { get; set; }
        public IEnumerable<Size> Sizes { get; set; }

    }
    public class ProductDisplayModel
    {
        public IEnumerable<ProductWithSizes> ProductSize { get; set; }
        public IEnumerable<Type> Type { get; set; }
        public IEnumerable<Material> Material { get; set; }

        public string STerm { get; set; } = "";
        public int TypeId { get; set; } = 0;

        public int SizeId { get; set; } = 0;
        public int MaterialId { get; set; } = 0;
    }
}
