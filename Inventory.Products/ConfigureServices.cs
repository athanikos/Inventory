using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Inventory.Products.Repositories;
using Inventory.Products.Services;

namespace Inventory.Products
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services, 
            IConfiguration configuration,
            List<System.Reflection.Assembly>
            mediatRAssemblies
            )
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<ProductsDbContext>(options =>  
                                                     options.UseNpgsql(configuration
                                                     .GetValue<string>("Products")));

            services.AddScoped<IInventoryRepository, PostgresInventoryRepository>();
            services.AddScoped<IModifyQuantityRepository, PostgresModifyQuantityRepository>();
            services.AddScoped<IModifyQuantityService, ModifyQuantityService>();
            services.AddScoped<IInventoryService, InventoryService>();

            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            return services;
        }
    }
}

