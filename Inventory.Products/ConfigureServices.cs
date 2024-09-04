using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Inventory.Products.Repositories;

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
             services.
                     AddDbContext<ProductsDbContext>(options =>  
                                               options.UseNpgsql(configuration
                                               .GetValue<String>("Products")));


            services.AddScoped<IInventoryRepository, InventoryRepository>();
            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            return services;
        }
    }
}

