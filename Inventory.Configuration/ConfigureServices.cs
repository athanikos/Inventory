using Inventory.Defaults.Repositories;
using Inventory.Defaults.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace Inventory.Defaults
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
            services.AddDbContext<ConfigurationDbContext>(options =>  
                                                     options.UseNpgsql(configuration
                                                     .GetValue<string>("Configuration")));
            
            
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            
            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            return services;
        }
    }
}

