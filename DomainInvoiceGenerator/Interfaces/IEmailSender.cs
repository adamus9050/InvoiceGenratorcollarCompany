using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task CreateTestMessage4(string emailTo, string attach,string server);
        Task ExcellDocGenerator(string filename, IEnumerable<CartDetail> orderDetails);
    }
}
