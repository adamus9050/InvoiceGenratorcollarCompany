using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Type
    {
        [Key]
        public int Id { get; set; }

        public string TypeName { get; set; }
        List<Product> Products { get; set; }
    }
}
