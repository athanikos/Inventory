
using Inventory.Products.Contracts;

namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Contracts.Dto;
    using Inventory.Products.Dto;
    using Inventory.Products.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteProduct(IInventoryRepository repo) : Endpoint<DeleteProductRequest>
    {
        public override void Configure()
        {
            Delete("/products");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteProductRequest req,
                        CancellationToken ct)
        {
            await repo.DeleteProductAsync(new ProductDto(req.Id, string.Empty,string.Empty, Guid.Empty, new List<ProductMetricDto>()));
            return TypedResults.Ok(); //todo fix 
            
        }
    }


    public record DeleteProductRequest
        (string Description, Guid Id);

  
}
