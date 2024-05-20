using InvoiceGeneratorCollarCompany.Seeder;
using Infrastructures.Extesions;
using Infrastructures.Context.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Infrastructures.Context;
;

var builder = WebApplication.CreateBuilder(args);



//var connectionString = builder.Configuration.GetConnectionString("InvoiceGeneratorCollarCompanyContextConnection"); //?? throw new InvalidOperationException("Connection string 'InvoiceGeneratorCollarCompanyContextConnection' not found.");

//builder.Services.AddDbContext<InvoiceGeneratorCollarCompanyContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<InvoiceGeneratorCollarCompanyContext>();


builder.Services.AddControllersWithViews();


//dodanie Infrastruktury do porjektu
builder.Services.AddInfrastructures(builder.Configuration);


var app = builder.Build();

//Dostarczenie DbSeeder 
using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedDefaultDataAsync(scope.ServiceProvider);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
