using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
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



            //todo do i need all these?
            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            //mediatRAssemblies.Add(typeof(Products.Contracts.AddProductMetricCommand).Assembly);
            //mediatRAssemblies.Add(typeof(Products.ConfigureServices).Assembly);

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

