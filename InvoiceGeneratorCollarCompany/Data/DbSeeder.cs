using InvoiceGeneratorCollarCompany.Areas.Constants;
using Microsoft.AspNetCore.Identity;

namespace InvoiceGeneratorCollarCompany.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultDataAsync(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            //dodanie ról do bazy danych
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            //logika tworzenia admina i usera
            var admin = new IdentityUser
            {
                UserName = "adamus9050@gmail.com",
                Email = "adamus9050@gmail.com",
                EmailConfirmed = true

            };

            var UserExistInDb = userMgr.FindByEmailAsync(admin.Email);

            if(UserExistInDb is null) 
            {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());

            }
        }
    }
}
