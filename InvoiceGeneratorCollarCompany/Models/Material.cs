using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;



namespace InvoiceGeneratorCollarCompany.Models
{
    public class Material
    {
        [Key]
        public int MaterialId { get; set; } = default!;

        public string Name { get; set; } = default!;
        public string Colour { get; set; } = default!;
        public string? ImageColour { get; set; }
        public string Description { get; set; } = default!;
        [Column(TypeName = "decimal(6,2)")]
        public double Price { get; set; }

    }
}
