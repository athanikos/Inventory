
namespace Inventory.Metrics.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Dto;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public  class AddMetric 
        : Endpoint<AddMetricRequest>
    {
        private readonly IMediator _mediator;

        public  AddMetric(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/Metric");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok<MetricDto>, NotFound, ProblemDetails>>
            HandleAsync(AddMetricRequest req,
                        CancellationToken ct)
        {
            var command = new AddMetricCommand(
                req.Description, req.InventoryId);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<MetricDto>(result);
        }
    }


    public record AddMetricRequest
        (string Description, Guid InventoryId);

    public record AddMetricCommand(string Description, 
        Guid InventoryId)
      : IRequest<MetricDto>;

  
}
