
using Inventory.Products.Contracts;

namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Contracts.Dto;
    using Inventory.Products.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetProducts(IInventoryRepository repo) : Endpoint<GetProductsRequest>
    {
        public override void Configure()
        {
            Get("/products");
            AllowAnonymous(); //todo remove 
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok<List<ProductDto>>, NotFound, ProblemDetails>>
            HandleAsync(GetProductsRequest req,
                        CancellationToken ct)
        {

            
            return TypedResults.Ok(await repo.GetProductsAsync());
        }
    }
    public record GetProductsRequest();

    
}
