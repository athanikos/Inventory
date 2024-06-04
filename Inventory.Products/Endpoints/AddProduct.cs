
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Dto;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public  class AddProduct 
        : Endpoint<AddProductRequest>
    {
        private readonly IMediator _mediator;

        public  AddProduct(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/product");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok<ProductDto>, NotFound, ProblemDetails>>
            HandleAsync(AddProductRequest req,
                        CancellationToken ct)
        {
            var command = new AddProductCommand(req.ProductId,   req.Description, req.InventoryId);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<ProductDto>(result);
        }
    }

    public record AddProductRequest
        (Guid ProductId, string Description, Guid InventoryId);

    public record AddProductCommand(Guid ProductId, string Description, Guid InventoryId)
      : IRequest<ProductDto>;

  
}
