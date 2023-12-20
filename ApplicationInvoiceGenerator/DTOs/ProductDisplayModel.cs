using Domain.Models;
using System.ComponentModel.DataAnnotations;
using Type = Domain.Models.Type;

namespace Application.DTOs
{
    public class ProductWithSizes
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Pole Nazwa jest obowiązkowe!")]

        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Pole Cena Jest obowiązkowe!")]

        public double ProductPrice { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage ="Podaj typ produktu")]
        public int  TypeId { get; set; }
        [Required(ErrorMessage = "Podaj typ produktu")]
        public Type Type { get; set; }
        public IEnumerable<Size> Sizes { get; set; }

    }

    public class ProductDisplayModel
    {
        public Product Product { get; set; }
        
        public IEnumerable<ProductWithSizes> ProductSize { get; set; }
        public IEnumerable<Type> Type { get; set; }
        public IEnumerable<Material> Material { get; set; }

        public string STerm { get; set; } = "";
        public int TypeId { get; set; } = 0;

        public int SizeId { get; set; } = 0;
        public int MaterialId { get; set; } = 0;
    }
}
