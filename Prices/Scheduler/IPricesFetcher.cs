using Inventory.Prices.Entities;

namespace Prices.Inventory.Prices
{
    public interface IPricesFetcher
    {
        void DoScedhuledWork();
        Task DoScheduledWork(PricesParameter p);
        void ScedhuleJobs(IServiceProvider serviceProvider);
    }
}