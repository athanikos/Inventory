using Inventory.Prices.Entities;

namespace Prices.Inventory.Prices
{
    public interface IPricesFetcher
    {
        void DoScedhuledWork();
        void DoScedhuledWork(PricesParameter p);
        void ScedhuleJobs();
    }
}