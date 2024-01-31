using Microsoft.EntityFrameworkCore;


namespace Domain.Models
{
    
    public class SizeProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = default;


        public int SizeId { get; set; }
        public Size Size { get; set; } = default;
    }
}
