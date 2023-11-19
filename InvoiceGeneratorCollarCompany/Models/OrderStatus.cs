using System.ComponentModel.DataAnnotations;

namespace InvoiceGeneratorCollarCompany.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }
        [Required]
        public int StatusId { get; set; }
        [MaxLength(20),Required]
        public string ?StatusName { get; set; }
    
        
    
    
    }
}
