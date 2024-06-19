﻿using Hangfire;
using Inventory.Prices;
using RestSharp;
using Entities = Inventory.Prices.Entities;
using Newtonsoft.Json.Linq;
using Inventory.Products.Contracts;
using MediatR;



namespace Prices
{
    namespace Inventory.Prices
    {
        public class PricesFetcher : IPricesFetcher
        {
            private string _parameterType = "COINGECKO";
           
            private readonly PricesDbContext _context;
            private readonly IMediator _mediator;

            private List<Entities.PricesParameter> Parameters { get; set; } =
                new List<Entities.PricesParameter>();

            public PricesFetcher(PricesDbContext context, IMediator mediator)
            { 
                _context = context;
                _mediator = mediator;
            }
            
            protected virtual List<Entities.PricesParameter> GetParameters()
            {
                if (string.IsNullOrEmpty(_parameterType))
                    throw new ArgumentNullException(nameof(_parameterType));
                return [.. _context.Parameters.Where(p => p.ParameterType == _parameterType)];
            }

            public void ScedhuleJobs(IServiceProvider serviceProvider)
            {
                Parameters = GetParameters();

                if (string.IsNullOrEmpty(_parameterType))
                    throw new ArgumentNullException(nameof(_parameterType));

                foreach (var p in Parameters)
                {
                        RecurringJob.AddOrUpdate(p.Id.ToString(), () => DoScheduledWork(p), Cron.Minutely);
                    
                }
                



            }

            public void DoScheduledWork(Entities.PricesParameter p)
            {

             
                   var options = new RestClientOptions(p.TargetURL + p.TargetProductCode);
                    var client = new RestClient(options);
                    
                    var request = new RestRequest("");
                    request.AddHeader("accept", "application/json");
                    request.AddHeader("x-cg-demo-api-key", p.TargetKey);
                
                    var response = client.Get(request);
                    JObject o = JObject.Parse(response.Content);
                    var value = decimal.Parse(o.SelectToken(p.TargetPathForProductCode).ToString());
                  
                
                    var command = new AddProductMetricCommand(p.ProductId, p.MetricId, value, DateTime.Now, p.TargetCurrency);
                    _mediator.Send(command);

            }


            public void DoScedhuledWork()
            {
                foreach (var p in GetParameters())
                    DoScheduledWork(p);
            }

        }

    }
}
