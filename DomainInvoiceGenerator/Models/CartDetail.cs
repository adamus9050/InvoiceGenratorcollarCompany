using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {

        public int Id { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        [Required]
        public int SizeProductId { get; set; }
        [Required]
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        public SizeProduct SizeProducts{ get; set; }
        public Material Materials{ get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }


    }
}
