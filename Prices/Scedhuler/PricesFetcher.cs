using Hangfire;
using Inventory.Prices;
using Inventory.Prices.Entities;
using RestSharp;
using Entities = Inventory.Prices.Entities;

namespace Prices
{
    namespace Inventory.Prices
    {
        public abstract class PricesFetcher
        {
            protected string _parameterType = string.Empty;
            protected readonly PricesDbContext _context;
            protected List<Entities.PricesParameter> Parameters { get; set; } =
                new List<Entities.PricesParameter>(); 

            public PricesFetcher(PricesDbContext context) => _context = context;

            protected virtual List<Entities.PricesParameter> GetParameters()
            {
                if (string.IsNullOrEmpty(_parameterType))
                    throw new ArgumentNullException(nameof(_parameterType));
                return [.. _context.Parameters.Where(p=>p.ParameterType == _parameterType)];    
            }
           
            internal  virtual void  ScedhuleJobs()
            {
                Parameters = GetParameters();

                if (string.IsNullOrEmpty(_parameterType))
                    throw new ArgumentNullException(nameof(_parameterType));

                foreach (var p in GetParameters())
                {
                     RecurringJob.AddOrUpdate(
                        p.Id.ToString(),
                        () => DoScedhuledWork(p),
                        Cron.Minutely);
                }
            }

            protected async virtual void DoScedhuledWork(Entities.PricesParameter p)
            {
                var options = new RestClientOptions("https://api.coingecko.com/api/v3/coins/" + "");// p.TargetProductCode);
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                // request.AddHeader("x-cg-demo-api-key", "CG-u6cMtcA6FGKaGxuJChCnCv5G\t");
           //     request.AddHeader("x-cg-demo-api-key",p.TargetKey);
                var response = await client.GetAsync(request);

            }


        }

    }
}
