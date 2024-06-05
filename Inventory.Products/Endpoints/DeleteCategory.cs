
namespace Category.Products.Endpoints
{
    using Azure.Core;
    using FastEndpoints;
    using Inventory.Products.Dto;
    using Inventory.Products.Repositories;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteCategory :
        Endpoint<DeleteCategoryRequest>
    {
        private readonly IInventoryRepository _repo;

        public  DeleteCategory(IInventoryRepository repo)
        {
            _repo = repo;
        }

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
             await  _repo.DeleteCategoryAsync(new CategoryDto(req.Id, string.Empty, Guid.Empty));
            return TypedResults.Ok();
        }
    }


    public record DeleteCategoryRequest(Guid Id);

    public record DeleteCategoryCommand(Guid Id)
      : IRequest;

  
}
