
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;

    public class EditProduct :
        Endpoint<EditProductRequest>
    {
        private readonly IMediator _mediator;

        public  EditProduct(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("/product");
            // to do claims this is per ProductId claim
            //  something like Admin_<ProductId>
        }

        public override async Task<Results<Ok<ProductDto>, NotFound, ProblemDetails>>
            HandleAsync(EditProductRequest req,
                        CancellationToken ct)
        {
            var command = new EditProductCommand(
              req.id,  req.Description, req.InventoryId);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<ProductDto>(result);
        }
    }


    public record EditProductRequest(Guid id, string Description, Guid InventoryId);

    public record EditProductCommand(Guid id,  string Description, Guid InventoryId)
      : IRequest<ProductDto>;

  
}
