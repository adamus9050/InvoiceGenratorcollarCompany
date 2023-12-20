using Infrastructures.Context;
using InvoiceGeneratorCollarCompany.Seeder;
using Microsoft.AspNetCore.Identity;
using Infrastructures.Repositories;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Domain.Interfaces;
using Infrastructures.Extesions;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("InvoiceGenerator") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

////Dodanie IdentityRole do DbContext 
//builder.Services
    //.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    //.AddEntityFrameworkStores<ApplicationDbContext>()
    //.AddDefaultUI()
    //.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();


//dodanie DBContext do porjektu
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