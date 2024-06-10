
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using global::Prices.Inventory.Prices;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public  class RunFetcher 
        : Endpoint <RunFetcherRequest>
    {

        private readonly IPricesFetcher _fetcher; 

        public RunFetcher(IPricesFetcher fetcher)
        {
            _fetcher = fetcher; 
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
            _fetcher.DoScedhuledWork();
            await Task.CompletedTask;
            return TypedResults.Ok();
        }


    }

    public record RunFetcherRequest(string Id );
}
