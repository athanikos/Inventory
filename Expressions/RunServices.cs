
using Expressions;
using Inventory.Expressions.Services;
using Microsoft.Extensions.DependencyInjection;



namespace Inventory.Expressions
{
    public static class RunServices
    {
        public static void Run(
            this IServiceCollection services 
            )
        {
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IEvaluatorService>();
            
            service?.ScheduleJobs(serviceProvider);
        }
    }
}

