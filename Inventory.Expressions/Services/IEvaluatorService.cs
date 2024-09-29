
using Expressions.Entities;

namespace Inventory.Expressions.Services
{
    public interface IEvaluatorService
    {
        void DoScheduledWork();
        void  DoScheduledWork(ProductExpression p);
        void ScheduleJobs(IServiceProvider serviceProvider);
    }
}