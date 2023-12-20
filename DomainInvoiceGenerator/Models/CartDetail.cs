using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        
        public int Id { get; set; }
        [Required]
        public int ShoppingCart_Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public int SizeId { get; set; }  //dodane sprawdzenie
        public Size Size { get; set; }
        public Product Products{ get; set; }
        public Material Materials{ get; set; }
        public ShoppingCart ShoppingCart { get; set; }

    }
}
