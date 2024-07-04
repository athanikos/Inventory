using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Prices.Inventory.Prices;
using Hangfire;
using Serilog;
using MediatR;
using Hangfire.PostgreSql;



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
            services.AddEntityFrameworkNpgsql().AddDbContext<PricesDbContext>(options =>
            options.UseNpgsql(configuration.
            GetConnectionString("Prices")));

            var str = configuration.GetConnectionString("Hangfire");

      
            services.AddHangfire(configuration => configuration
                  .UsePostgreSqlStorage(str)
                );

            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            mediatRAssemblies.Add(typeof(Products.Contracts.AddProductMetricCommand).Assembly);
            mediatRAssemblies.Add(typeof(Products.ConfigureServices).Assembly);


           

             Log.Logger  = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/Net6Tester.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            services.AddScoped<IPricesFetcher, PricesFetcher>(
            sp =>
            {   
                return new PricesFetcher(sp.GetRequiredService<PricesDbContext>(), 
                                          sp.GetRequiredService<IMediator>()
                                          );
            }
            );

            services.AddHangfireServer();


            return services;
        }

  
    }
}

