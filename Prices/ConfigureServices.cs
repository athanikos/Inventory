﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Hangfire;
using MediatR;
using Hangfire.MemoryStorage;
using Inventory.Prices.Repositories;
using Inventory.Prices.Services;
using Inventory.Prices.Services.Inventory.Prices;


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

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<PricesDbContext>(options =>
            options.UseNpgsql(configuration
            .GetValue<String>("Prices")));

        
            services.AddHangfire(c=>c.UseMemoryStorage());

            JobStorage.Current = new MemoryStorage();

            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            mediatRAssemblies.Add(typeof(Products.Contracts.AddProductMetricCommand).Assembly);
            mediatRAssemblies.Add(typeof(Products.ConfigureServices).Assembly);


            services.AddScoped<IFetcherRepository, PostgresFetcherRepository>();


            // Log.Logger  = new LoggerConfiguration()
            //.MinimumLevel.Debug()
            //.WriteTo.File("logs/Net6Tester.txt", rollingInterval: RollingInterval.Day)
            //.CreateLogger();

            services.AddScoped<IPricesService, PricesService>(
            sp =>
            {   
                return new PricesService(sp.GetRequiredService<IFetcherRepository>(), 
                                          sp.GetRequiredService<IMediator>()
                                          );
            }
            );

            services.AddHangfireServer();


            return services;
        }

  
    }
}

