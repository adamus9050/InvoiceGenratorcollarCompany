using Microsoft.EntityFrameworkCore;


namespace Domain.Models
{
    [Keyless]
    public class SizeProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }


        public int SizeId { get; set; }
        public Size Size { get; set; }
    }
}
