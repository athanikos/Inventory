using Inventory.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Notifier
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services, 
            IConfiguration configuration,
            List<System.Reflection.Assembly>  mediatRAssemblies
            )
        {


            services.AddDbContext<NotifierDbContext>(options =>
            options.UseSqlServer(configuration.
            GetConnectionString("Notifications")));


            
            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);

            return services;
        }

  
    }
}

