using InvoiceGeneratorCollarCompany.Seeder;
using Infrastructures.Extesions;

var builder = WebApplication.CreateBuilder(args);

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
