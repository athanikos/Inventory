using Inventory.Prices.Entities;

namespace Prices.Inventory.Prices
{
    public interface IPricesFetcher
    {
        Task DoScedhuledWork();
        Task DoScheduledWork(PricesParameter p);
        void ScedhuleJobs(IServiceProvider serviceProvider);
    }
}