
namespace Inventory.Metrics.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Contracts.Dto;
    using Inventory.Products.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteMetric(IInventoryRepository repo) : Endpoint<DeleteMetricRequest>
    {
        public override void Configure()
        {
            Delete("/metrics");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteMetricRequest req,
                        CancellationToken ct)
        {
            await repo.DeleteMetricAsync(new MetricDto(req.Id));
            return TypedResults.Ok();
        }
    }


    public record DeleteMetricRequest (Guid Id);

  
  
}
