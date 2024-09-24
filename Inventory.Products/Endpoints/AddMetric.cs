
namespace Inventory.Metrics.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Contracts.Dto;
    using Inventory.Products.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public  class AddMetric 
        : Endpoint<AddMetricRequest>
    {
        private readonly IInventoryRepository _repo;

        public  AddMetric(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Post("/Metric");
            AllowAnonymous(); //todo remove 

            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok<MetricDto>, NotFound, ProblemDetails>>
            HandleAsync(AddMetricRequest req,
                        CancellationToken ct)
        {
            
            
            var dto =   await _repo.AddMetricAsync(new MetricDto(req.Id,
                                                                 req.Description,  
                                                                 req.Code, 
                                                                 req.SourceId));


            return TypedResults.Ok<MetricDto>(dto);
        }
    }

    public record AddMetricRequest (Guid Id, string Description,string Code,  Guid SourceId);

  
  
}
