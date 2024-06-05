
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;
    using Inventory.Products.Repositories;

    public class EditMetric :
        Endpoint<EditMetricRequest>
    {
        private readonly IInventoryRepository _repo;

        public  EditMetric(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Put("/metric");
            // to do claims this is per MetricId claim
            //  something like Admin_<MetricId>
        }

        public override async Task<Results<Ok<MetricDto>, NotFound, ProblemDetails>>
            HandleAsync(EditMetricRequest req,
                        CancellationToken ct)
        {
        
            var dto =  await _repo.EditMetricAsync(new MetricDto(req.Id, req.Description, req.Value, 
                req.EffectiveDate, req.Code, req.SourceId));

            return TypedResults.Ok(dto);
        }
    }
    public record EditMetricRequest(Guid Id,
    string Description,
    decimal Value,
    DateTime EffectiveDate,
    string Code,
    Guid SourceId);
   
  
  
}
