using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Context.Data;

public class InvoiceGeneratorCollarCompanyContext : IdentityUser
{




    [PersonalData]
    public string Name { get; set; }
    [PersonalData]
    public string Surname { get; set; }
    [PersonalData]
    public string CompanyName { get; set; }
    [PersonalData]
    public string Street { get; set; }
    [PersonalData]
    public string NumberOf { get; set; }
    [PersonalData]
    public string  PostCode { get; set; }
}
