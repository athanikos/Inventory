
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
            Put("/Metric");
            // to do claims this is per MetricId claim
            //  something like Admin_<MetricId>
        }

        public override async Task<Results<Ok<MetricDto>, NotFound, ProblemDetails>>
            HandleAsync(EditMetricRequest req,
                        CancellationToken ct)
        {
            var command = new EditMetricCommand(
                req.Description);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<MetricDto>(result);
        }
    }


    public record EditMetricRequest(string Description);

    public record EditMetricCommand(string Description)
      : IRequest<MetricDto>;

  
}
