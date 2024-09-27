
using Inventory.Prices.Services;

namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public  class RunFetcher 
        : Endpoint <RunFetcherRequest>
    {

        private readonly IPricesService _service; 

        public RunFetcher(IPricesService service)
        {
            _service = service; 
        }

        public override void Configure()
        {
            Post("/PricesFetcher");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override  async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(RunFetcherRequest req,
                        CancellationToken ct)
        {
            await _service.DoScheduledWork();
            return TypedResults.Ok();
        }


    }

    public record RunFetcherRequest(string Id );
}
