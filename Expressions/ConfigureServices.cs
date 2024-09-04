using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MediatR;
using Expressions;
using Inventory.Expressions.Repositories;

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

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<ExpressionsDbContext>(options =>
            options.UseNpgsql(configuration
            .GetValue<String>("Expressions")));



            //todo do i need all these?
            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            //mediatRAssemblies.Add(typeof(Products.Contracts.AddProductMetricCommand).Assembly);
            //mediatRAssemblies.Add(typeof(Products.ConfigureServices).Assembly);

            services.AddScoped<IEvaluator, Evaluator>(
            sp =>
            {
                return new Evaluator(sp.GetRequiredService<IMediator>(), sp.GetRequiredService<IExpressionRepository>());
            }
            );
                               
            return services;
        }

  
    }
}

