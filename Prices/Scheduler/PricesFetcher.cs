﻿using Hangfire;
using Inventory.Prices;
using RestSharp;
using Entities = Inventory.Prices.Entities;
using Newtonsoft.Json.Linq;
using Inventory.Products.Contracts;
using MediatR;
using Serilog;
using Inventory.Prices.Repositories;


namespace Prices
{
    namespace Inventory.Prices
    {
        public class PricesFetcher : IPricesFetcher
        {
            private string _parameterType = "COINGECKO";
           
            private readonly IFetcherRepository _repository;
            private readonly IMediator _mediator;

            private List<Entities.PricesParameter> Parameters { get; set; } =   new List<Entities.PricesParameter>();

            public PricesFetcher(IFetcherRepository repository, IMediator mediator)
            {
                _repository = repository;
                _mediator = mediator;
            }
            
            public void ScedhuleJobs(IServiceProvider serviceProvider)
            {
                Parameters = _repository.GetParameters();

                if (string.IsNullOrEmpty(_parameterType))
                    throw new ArgumentNullException(nameof(_parameterType));

                foreach (var p in Parameters)
                        RecurringJob.AddOrUpdate(p.Id.ToString(), () => DoScheduledWork(p), Cron.Minutely);
            }

            public async Task DoScheduledWork(Entities.PricesParameter p)
            {
                try
                {
                    // Log.Information("DoScheduledWork " + p.Id.ToString());

                    var options = new RestClientOptions(p.TargetURL + p.TargetProductCode);
                    var client = new RestClient(options);
                    var request = new RestRequest("");
                    request.AddHeader("accept", "application/json");
                    request.AddHeader("x-cg-demo-api-key", p.TargetKey);
                    // Log.Information(" client.Get(request) ");

                    var response = client.Get(request);
                    JObject o = JObject.Parse(response.Content);
                    var value = decimal.Parse(o.SelectToken(p.TargetPathForProductCode).ToString());
                    var command = new AddProductMetricCommand(p.ProductId, p.MetricId,
                        value, DateTime.Now, p.TargetCurrency);

                    // Log.Information("AddProductMetricCommand  _mediator.Send " + p.ProductId + " " + p.MetricId);

                   await  _mediator.Send(command);
                    
                    // Log.Information("After mediator.send ");

                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());   
                }
                

            }


            public async Task  DoScedhuledWork()
            {

                var items = _repository.    GetParameters();
                foreach (var p in items)
                  await  DoScheduledWork(p);
            }

        }

    }
}
