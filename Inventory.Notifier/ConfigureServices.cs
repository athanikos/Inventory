using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Notifications
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services, 
            IConfiguration configuration,
            List<System.Reflection.Assembly>  mediatRAssemblies
            )
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<NotifierDbContext>(options =>
            options.UseNpgsql(configuration
            .GetValue<String>("Notifications")));

            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            return services;
        }

  
    }
}

