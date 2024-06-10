

using Hangfire;
using Inventory.Prices;
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
            GetConnectionString("Prices")));


            var str = configuration.GetConnectionString("Hangfire");

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(str)
                );

            GlobalConfiguration.Configuration
            .UseSqlServerStorage("Server=localhost;Database=Hangfire;Integrated Security=SSPI;Encrypt=False;");

            services.AddHangfireServer();
            
            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);

            return services;
        }

  
    }
}

