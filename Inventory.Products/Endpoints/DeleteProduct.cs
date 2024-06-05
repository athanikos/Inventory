
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Dto;
    using Inventory.Products.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public  class DeleteProduct 
        : Endpoint<DeleteProductRequest>
    {
        private readonly IInventoryRepository _repo;

        public  DeleteProduct(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Delete("/product");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteProductRequest req,
                        CancellationToken ct)
        {
            await _repo.DeleteProductAsync(new ProductDto(req.Id, string.Empty, Guid.Empty));
            return TypedResults.Ok(); //todo fix 
            
        }
    }


    public record DeleteProductRequest
        (string Description, Guid Id);

  
}
