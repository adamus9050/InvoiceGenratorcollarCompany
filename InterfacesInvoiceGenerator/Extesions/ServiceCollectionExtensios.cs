using Domain.Interfaces;
using Domain.Interfces;
using Infrastructures.Context;
using Infrastructures.Context.Data;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Extesions
{
    public static class ServiceCollectionExtensios
    {

        public static void AddInfrastructures(this IServiceCollection services, IConfiguration configuration)
        {
            //Wstrzykiwanie AppliccationDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("InvoiceGenerator"))
                 .UseLoggerFactory(ApplicationDbContext._loggerFactory)  // Dodanie logowania
                 .EnableSensitiveDataLogging());  // Logowanie danych wrażliwych;





            //Wstrzykiwanie Identity User
            services.AddIdentity<InvoiceGeneratorCollarCompanyContext, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultUI()
                .AddDefaultTokenProviders();

            //Wstrzykiwanie Interfejsów
            services.AddTransient<IHomeRepository, HomeRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddTransient<ICrudRepository, CrudRepository>();
            services.AddScoped<IUserOrderRepository, UserOrderRepository>();
            services.AddTransient<IEmailSender, EmailSender>();

            
            services.AddMvc();
        }
    }
}
