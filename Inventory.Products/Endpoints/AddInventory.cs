
namespace Inventory.Products.Endpoints
{
    using Azure;
    using FastEndpoints;
    using Inventory.Products.Dto;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddInventory :
        Endpoint<AddInventoryRequest>
    {
        private readonly IMediator _mediator;

        public  AddInventory(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/inventory");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok<InventoryDto>, NotFound, ProblemDetails>>
            HandleAsync(AddInventoryRequest req,
                        CancellationToken ct)
        {
            var command = new AddInventoryCommand(
                req.Description);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<InventoryDto>(result);
        }
    }


    public record AddInventoryRequest(string Description);

    public record AddInventoryCommand(string Description)
      : IRequest<InventoryDto>;

  
}
