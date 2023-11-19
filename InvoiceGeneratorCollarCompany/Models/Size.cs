using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGeneratorCollarCompany.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }
        public string Namestring { get; set; }
        public int NameInt { get; set; }

    }
}
