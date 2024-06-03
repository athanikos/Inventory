
namespace Inventory.Metrics.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Dto;
    using Inventory.Products.Entities;
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
                        req.Id,
                        req.Description,
                        req.Value,
                        req.EffectiveDate,
                        req.Code,
                        req.SourceId);

            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<MetricDto>(result);
        }
    }


    public record AddMetricRequest
        (Guid Id,
                        string Description,
                        decimal Value,
                        DateTime EffectiveDate,
                        string Code,
                        Guid SourceId);

    public record AddMetricCommand(Guid Id,
                        string Description,
                        decimal Value,
                        DateTime EffectiveDate,
                        string Code,
                        Guid SourceId)
      : IRequest<MetricDto>;

  
}
