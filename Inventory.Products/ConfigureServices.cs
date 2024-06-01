using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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
            services.AddDbContext<ProductsDbContext>(options =>  
            options.UseSqlServer(configuration.
            GetConnectionString("Products")));

            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            return services;
        }
    }
}

