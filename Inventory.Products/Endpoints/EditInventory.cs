
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Dto;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class EditInventory :
        Endpoint<EditInventoryRequest>
    {
        private readonly IMediator _mediator;

        public  EditInventory(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("/inventory");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok<InventoryDto>, NotFound, ProblemDetails>>
            HandleAsync(EditInventoryRequest req,
                        CancellationToken ct)
        {
            var command = new EditInventoryCommand(
                req.Description);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<InventoryDto>(result);
        }
    }


    public record EditInventoryRequest(string Description);

    public record EditInventoryCommand(string Description)
      : IRequest<InventoryDto>;

  
}
