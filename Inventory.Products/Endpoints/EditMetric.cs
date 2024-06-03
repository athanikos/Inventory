
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;

    public class EditMetric :
        Endpoint<EditMetricRequest>
    {
        private readonly IMediator _mediator;

        public  EditMetric(IMediator mediator)
        {
            _mediator = mediator;
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
            var command = new EditMetricCommand(
             req. Id,
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


    public record EditMetricRequest(Guid Id,
                        string Description,
                        decimal Value,
                        DateTime EffectiveDate,
                        string Code,
                        Guid SourceId);

    public record EditMetricCommand(Guid Id,
                        string Description,
                        decimal Value,
                        DateTime EffectiveDate,
                        string Code,
                        Guid SourceId)  : IRequest<MetricDto>;

  
}
