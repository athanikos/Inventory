using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Inventory.Transactions.Repositories;

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

            services.AddEntityFrameworkNpgsql().AddDbContext<TransactionsDbContext>(options =>
            options.UseNpgsql(configuration.
            GetConnectionString("Transactions")));

            services.AddScoped<ITransactionRepository, TransactionRepository>();
       

            mediatRAssemblies.Add(typeof(ConfigureServices).Assembly);
            return services;
        }
    }
}

