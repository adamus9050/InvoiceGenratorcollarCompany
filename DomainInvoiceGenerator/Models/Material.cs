using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }

        [Required(ErrorMessage ="Pole Nazwa jest obowiązkowe!")]
        [StringLength(13, MinimumLength = 3)]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Pole Kolor jest obowiązkowe!")]
        [StringLength(9, MinimumLength = 5)]
        public string Colour { get; set; } = default!;
        public string? ImageColour { get; set; }
        public string Description { get; set; } = default!;

        [Column(TypeName = "decimal(6,2)")]
        [Required(ErrorMessage = "Pole Cena jest obowiązkowe!")]
        public double Price { get; set; }

        public ICollection<CartDetail> CartDetails { get; set; }



    }
}
