using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Models
{
    public class Product
    {
        [Key]
        
        public int ProductId { get; set; }
       // [Required(ErrorMessage ="Pole Nazwa jest obowiązkowe!")]
        public string ProductName { get; set; }

        public string Description { get; set; }
       // [Required(ErrorMessage ="Pole Cena Jest obowiązkowe!")]
        public double ProductPrice { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }


        public int TypeId { get; set; }

       // [Required(ErrorMessage = "Podaj typ produktu!")]
        public Type Type { get; set; }       
        
        [NotMapped]
        public string TypeName { get; set; }
        public IEnumerable<Material> Materials { get; set; }

    }
}
