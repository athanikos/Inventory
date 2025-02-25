using Inventory.Transactions.Repositories;
using Inventory.Transactions.Repositories.Postgres;
using Inventory.Transactions.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Transactions
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

            services.AddDbContext<TransactionsDbContext>(options =>
            options.UseNpgsql(configuration
            .GetValue<string>("Transactions")));

            services.AddScoped<ITransactionRepository, PostgresTransactionRepository>();

            services.AddScoped<ITransactionService, TransactionsService>();
               
            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            return services;
        }
    }
}

