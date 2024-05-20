using Infrastructures.Context.Data;
using InvoiceGeneratorCollarCompany.Areas.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceGeneratorCollarCompany.Seeder
{
    public class DbSeeder
    {
        public static async Task SeedDefaultDataAsync(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<InvoiceGeneratorCollarCompanyContext>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            //dodanie ról do bazy danych
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            //logika tworzenia admina i usera
            var admin = new InvoiceGeneratorCollarCompanyContext
            {
                UserName = "adamus9050@gmail.com",
                Email = "adamus9050@gmail.com",
                EmailConfirmed = true            //hasło: Password1!

            };

            var UserExistInDb = userMgr.FindByEmailAsync(admin.Email);

            if (UserExistInDb is null)
            {
                await userMgr.CreateAsync(admin, "adamus9050@gmail.com");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());

            }
        }
    }
}
