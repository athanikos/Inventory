
using Inventory.Products.Contracts;

namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;
    using Inventory.Products.Repositories;
    using Inventory.Products.Contracts.Dto;

    public class EditProduct(IInventoryRepository repo) :
        Endpoint<EditProductRequest>
    {
        public override void Configure()
        {
            Put("/products");
            AllowAnonymous();
            // to do claims this is per ProductId claim
            //  something like Admin_<ProductId>
        }

        public override async Task<Results<Ok<ProductDto>, NotFound, ProblemDetails>>
            HandleAsync(EditProductRequest req,
                        CancellationToken ct)
        {
            var dto = new ProductDto(req.id, req.Description, req.Code, req.InventoryId, req.Metrics    );
            if (!repo.InventoryIdExists(req.InventoryId))
            {
                AddError("Inventory Id does not exist ");
                ThrowIfAnyErrors(); // If there are errors, execution shouldn't go beyond this point
                return new ProblemDetails(ValidationFailures);
            }

            //if (_repo.ProductDescriptionOrCategoryIsUsed(dto))
            //{
            //    AddError("Product code or descr is used  ");
            //    ThrowIfAnyErrors(); // If there are errors, execution shouldn't go beyond this point
            //    return new ProblemDetails(ValidationFailures);
            //}

            dto =   await repo.EditProductAsync(dto);
            return TypedResults.Ok<ProductDto>(dto);
        }
    }
    public record EditProductRequest(Guid id, string Description,string Code,  Guid InventoryId, List<ProductMetricDto> Metrics);

  
}
