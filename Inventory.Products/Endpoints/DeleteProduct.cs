
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Dto;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public  class DeleteProduct 
        : Endpoint<DeleteProductRequest>
    {
        private readonly IMediator _mediator;

        public  DeleteProduct(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Delete("/product");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok<ProductDto>, NotFound, ProblemDetails>>
            HandleAsync(DeleteProductRequest req,
                        CancellationToken ct)
        {
            var command = new DeleteProductCommand(
                req.Description, req.InventoryId);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<ProductDto>(result);
        }
    }


    public record DeleteProductRequest
        (string Description, Guid InventoryId);

    public record DeleteProductCommand(string Description, 
        Guid InventoryId)
      : IRequest<ProductDto>;

  
}
