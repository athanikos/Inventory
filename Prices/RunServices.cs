﻿using Inventory.Prices.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Inventory.Prices
{
    public static class RunServices
    {
        public static void Run(
            this IServiceCollection services 
            )
        {
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.
                GetRequiredService<IPricesService>();

            service.ScheduleJobs(serviceProvider);
        }


       

    }
}

