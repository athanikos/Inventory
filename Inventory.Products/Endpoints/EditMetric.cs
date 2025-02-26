
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Repositories;
    using Inventory.Products.Contracts.Dto;

    public class EditMetric(IInventoryRepository repo) :
        Endpoint<EditMetricRequest>
    {
        public override void Configure()
        {
            Put("/metrics");
            // to do claims this is per MetricId claim
            //  something like Admin_<MetricId>
        }

        public override async Task<Results<Ok<MetricDto>, NotFound, ProblemDetails>>
            HandleAsync(EditMetricRequest req,
                        CancellationToken ct)
        {
        
            var dto =  await repo.EditMetricAsync(new MetricDto(
                req.Id, req.Description, req.Code, req.SourceId));

            return TypedResults.Ok(dto);
        }
    }
    public record EditMetricRequest(Guid Id,
    string Description,
    string Code,
    Guid SourceId);
   
  
  
}
