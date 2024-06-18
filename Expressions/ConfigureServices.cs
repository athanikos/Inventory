using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Hangfire;
using Serilog;
using MediatR;
using Expressions;

namespace Inventory.Expressions
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services, 
            IConfiguration configuration,
            List<System.Reflection.Assembly>  mediatRAssemblies
            )
        {
            services.AddDbContext<ExpressionsDbContext>(options =>
            options.UseSqlServer(configuration.
            GetConnectionString("Expressions")));


            // allready added in Prices 
            //var str = configuration.GetConnectionString("Hangfire");

            //services.AddHangfire(configuration => configuration
            //    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            //    .UseSimpleAssemblyNameTypeSerializer()
            //    .UseRecommendedSerializerSettings()
            //    .UseSqlServerStorage(str)
            //    );

            //GlobalConfiguration.Configuration
            //.UseSqlServerStorage(str);
            //services.AddHangfireServer();


            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            mediatRAssemblies.Add(typeof(Products.Contracts.AddProductMetricCommand).Assembly);
            mediatRAssemblies.Add(typeof(Products.ConfigureServices).Assembly);


            var logger  = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/Net6Tester.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            services.AddScoped<IEvaluator, Evaluator>(
            sp =>
            {
                return new Evaluator(sp.GetRequiredService<IMediator>(), sp.GetRequiredService<ExpressionsDbContext>());
            }
            );
                               
            return services;
        }

  
    }
}

