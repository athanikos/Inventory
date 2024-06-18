
using Expressions.Entities;

namespace Expressions
{
    public interface IEvaluator
    {
        void DoScedhuledWork();
        void DoScedhuledWork(ProductExpression p);
        void ScedhuleJobs();

        Task<string> Execute(Guid inventoryId, string expression);


    }
}