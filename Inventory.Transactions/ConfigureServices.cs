using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Inventory.Transactions.Repositories;
using Inventory.Transactions.Repositories.Postgres;

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
            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            return services;
        }
    }
}

