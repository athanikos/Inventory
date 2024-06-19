using Microsoft.Extensions.DependencyInjection;
using Prices.Inventory.Prices;


namespace Inventory.Prices
{
    public static class RunServices
    {
        public static void Run(
            this IServiceCollection services 
            )
        {
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IPricesFetcher>();
            service.ScedhuleJobs(serviceProvider);
        }


       

    }
}

