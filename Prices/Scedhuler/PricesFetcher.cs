using Hangfire;
using Inventory.Prices;
using RestSharp;
using Entities = Inventory.Prices.Entities;
using Microsoft.Extensions.Logging;
using Azure.Core;



namespace Prices
{
    namespace Inventory.Prices
    {
        public class PricesFetcher : IPricesFetcher
        {
            private string _parameterType = "COINGECKO";
           
            private readonly PricesDbContext _context;
            private readonly Serilog.ILogger _logger;   

            private List<Entities.PricesParameter> Parameters { get; set; } =
                new List<Entities.PricesParameter>();

            public PricesFetcher(PricesDbContext context, Serilog.ILogger logger)
            { 
                _logger = logger;   
                _context = context; 
            }
            
            protected virtual List<Entities.PricesParameter> GetParameters()
            {
                if (string.IsNullOrEmpty(_parameterType))
                    throw new ArgumentNullException(nameof(_parameterType));
                return [.. _context.Parameters.Where(p => p.ParameterType == _parameterType)];
            }

            public void ScedhuleJobs()
            {
                Parameters = GetParameters();

                if (string.IsNullOrEmpty(_parameterType))
                    throw new ArgumentNullException(nameof(_parameterType));

                foreach (var p in GetParameters())
                {
                    RecurringJob.AddOrUpdate(p.Id.ToString(), () => DoScedhuledWork(p), Cron.Minutely);
                }
            }
            /// <summary>
            ///             //    request.AddHeader("x-cg-demo-api-key", "CG-u6cMtcA6FGKaGxuJChCnCv5G\t");
            /// </summary>
            /// <param name="p"></param>
            public void DoScedhuledWork(Entities.PricesParameter p)
            {
                try
                {

                    _logger.Information("in DoScedhuledWork");


                    var options = new RestClientOptions(p.TargetURL + p.TargetProductCode);
                    var client = new RestClient(options);
                    var request = new RestRequest("");
                    request.AddHeader("accept", "application/json");
                    request.AddHeader("x-cg-demo-api-key", p.TargetKey);
                    var response = client.Get(request);

                    _logger.Information(request?.ToString());
                    _logger.Information(response.ToString());
                }
                catch (Exception ex)
                {
                    _logger.Information("error"  + ex.Message.ToString());
                }

            }


            public void DoScedhuledWork()
            {
                foreach (var p in GetParameters())
                    DoScedhuledWork(p);
            }


        }

    }
}
