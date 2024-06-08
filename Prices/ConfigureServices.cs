using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Hangfire;
using Prices.Inventory.Prices;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;

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
            services.AddHangfire(
                        (sp, config) =>
                        {
                            var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("Hangfire");
                            config.UseSqlServerStorage(connectionString);
                        });

            services.AddHangfireServer();
            
            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            services.AddScoped< PricesFetcher ,CoinGeckoPricesFetcher  >();

            services.AddDbContext<PricesDbContext>(options =>
                                                   options.UseSqlServer(configuration.GetConnectionString("Prices")));

            // Build the intermediate service provider
            var sp = services.BuildServiceProvider();
            sp.GetService<PricesFetcher>().ScedhuleJobs();
          



            return services;
        }
    }
}

