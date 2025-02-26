using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;

namespace Inventory.Products.Endpoints
{
    public class EditCategory(IInventoryRepository repo) :
        Endpoint<EditCategoryRequest>
    {
        public override void Configure()
        {
            Put("/categories");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok<CategoryDto>, NotFound, ProblemDetails>>
            HandleAsync(EditCategoryRequest req,
                        CancellationToken ct)
        {

            var dto = await repo.AddCategoryAsync(new CategoryDto(req.Id, req.Description,req.FatherId) );

            return TypedResults.Ok(dto);
        }
    }


    public record EditCategoryRequest
        (Guid Id, string Description, Guid FatherId);

}
