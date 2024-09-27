using Inventory.Prices.Entities;

namespace Inventory.Prices.Services
{
    public interface IPricesService
    {
        Task DoScheduledWork();
        Task DoScheduledWork(PricesParameter p);
        void ScheduleJobs(IServiceProvider serviceProvider);
    }
}