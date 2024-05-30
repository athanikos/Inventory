
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    internal class AddProduct : Endpoint<AddProductRequest>
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

        public override async Task 
            HandleAsync(AddProductRequest req,
                        CancellationToken ct)
        {
            var command = new AddProductCommand(
                req.Description);
            var result = await _mediator!.
                Send(command, ct);

         
        }
    }


    public record AddProductRequest(string Description);

    public record AddProductCommand(string Description)
      : IRequest<ProductDto>;

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
