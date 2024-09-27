using FastEndpoints;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inventory.Products.Endpoints
{
    public class DeleteCategory(IInventoryRepository repo) :
        Endpoint<DeleteCategoryRequest>
    {
        public override void Configure()
        {
            Delete("/category");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteCategoryRequest req,
                        CancellationToken ct)
        {
             await  repo.DeleteCategoryAsync(new CategoryDto(req.Id, string.Empty, Guid.Empty));
            return TypedResults.Ok();
        }
    }


    public record DeleteCategoryRequest(Guid Id);

   
  
}
