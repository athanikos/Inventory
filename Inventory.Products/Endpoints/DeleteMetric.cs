
namespace Inventory.Metrics.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Dto;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public  class DeleteMetric 
        : Endpoint<DeleteMetricRequest>
    {
        private readonly IMediator _mediator;

        public  DeleteMetric(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Delete("/Metric");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok<MetricDto>, NotFound, ProblemDetails>>
            HandleAsync(DeleteMetricRequest req,
                        CancellationToken ct)
        {
            var command = new DeleteMetricCommand(
                req.Description, req.InventoryId);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<MetricDto>(result);
        }
    }


    public record DeleteMetricRequest
        (string Description, Guid InventoryId);

    public record DeleteMetricCommand(string Description, 
        Guid InventoryId)
      : IRequest<MetricDto>;

  
}
