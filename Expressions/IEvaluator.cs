
using Expressions.Entities;

namespace Expressions
{
    public interface IEvaluator
    {
        void DoScheduledWork();
        void  DoScheduledWork(ProductExpression p);
        void ScheduleJobs(IServiceProvider serviceProvider);
    }
}