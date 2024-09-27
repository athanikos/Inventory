using Hangfire;
using Inventory.Prices.Repositories;
using Inventory.Products.Contracts;
using MediatR;
using Newtonsoft.Json.Linq;
using RestSharp;
using Serilog;


namespace Inventory.Prices.Services
{
    namespace Inventory.Prices
    {
        public class PricesService(IFetcherRepository repository, IMediator mediator) : IPricesService
        {
            private string _parameterType = "COINGECKO";

            private List<Entities.PricesParameter> Parameters { get; set; } =   new();

            public void ScheduleJobs(IServiceProvider serviceProvider)
            {
                Parameters = repository.GetParameters();

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
                        value, DateTime.Now, Constants.CurrencyUnityOfMeasurementId);

                    // Log.Information("AddProductMetricCommand  _mediator.Send " + p.ProductId + " " + p.MetricId);

                   await  mediator.Send(command);
                    
                    // Log.Information("After mediator.send ");

                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());   
                }
                

            }


            public async Task  DoScheduledWork()
            {

                var items = repository.    GetParameters();
                foreach (var p in items)
                  await  DoScheduledWork(p);
            }

        }

    }
}
