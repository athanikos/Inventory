
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
            Delete("/metric");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteMetricRequest req,
                        CancellationToken ct)
        {
            var command = new DeleteMetricCommand(
                req.Id);
             await _mediator!.
                Send(command, ct);

            return TypedResults.Ok();
        }
    }


    public record DeleteMetricRequest (Guid Id);

    public record DeleteMetricCommand (Guid Id) : IRequest;

  
}
