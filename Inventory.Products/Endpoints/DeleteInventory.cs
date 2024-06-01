
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

    public class DeleteInventory :
        Endpoint<DeleteInventoryRequest>
    {
        private readonly IMediator _mediator;

        public  DeleteInventory(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Delete("/inventory");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok<InventoryDto>, NotFound, ProblemDetails>>
            HandleAsync(DeleteInventoryRequest req,
                        CancellationToken ct)
        {
            var command = new DeleteInventoryCommand(
                req.Description);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<InventoryDto>(result);
        }
    }


    public record DeleteInventoryRequest(string Description);

    public record DeleteInventoryCommand(string Description)
      : IRequest<InventoryDto>;

  
}
