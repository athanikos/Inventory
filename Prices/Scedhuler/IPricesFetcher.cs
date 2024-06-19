using Inventory.Prices.Entities;

namespace Prices.Inventory.Prices
{
    public interface IPricesFetcher
    {
        void DoScedhuledWork();
        void DoScheduledWork(PricesParameter p);
        void ScedhuleJobs(IServiceProvider serviceProvider);
    }
}