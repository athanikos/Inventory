using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Hangfire;
using Prices.Inventory.Prices;

namespace Inventory.Prices
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services, 
            IConfiguration configuration,
            List<System.Reflection.Assembly>  mediatRAssemblies
            )
        {


            services.AddDbContext<PricesDbContext>(options =>
            options.UseSqlServer(configuration.
            GetConnectionString("Prices")));

            services.AddHangfire(
                        (sp, config) =>
                        {
                            var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("Hangfire");
                            config.UseSqlServerStorage(connectionString);
                        });

            services.AddHangfireServer();
            
            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);



            services.AddSingleton< PricesFetcher , PricesFetcher>(
                sp =>
                {
                return new CoinGeckoPricesFetcher(sp.GetService<PricesDbContext>());
                });



            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<PricesFetcher>();
            //service?.ScedhuleJobs();


            return services;
        }
    }
}

