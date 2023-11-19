using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace InvoiceGeneratorCollarCompany.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }

        public string Description { get; set; }
        [Required]
        public double ProductPrice { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }

        public int TypeId { get; set; }
        public  Type Type { get; set; }       
        
        [NotMapped]
        public string TypeName { get; set; }
        public IEnumerable<Material> Materials { get; set; }

    }
}
