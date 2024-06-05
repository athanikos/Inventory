
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;
    using Inventory.Products.Repositories;

    public class EditProduct :
        Endpoint<EditProductRequest>
    {
        private readonly IInventoryRepository _repo;

        public  EditProduct(IInventoryRepository repo)
        {
            _repo = repo;
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
            var dto =   await _repo.EditProductAsync(new ProductDto(req.id, req.Description,req.InventoryId));
            return TypedResults.Ok<ProductDto>(dto);
        }
    }
    public record EditProductRequest(Guid id, string Description, Guid InventoryId);

  
}
