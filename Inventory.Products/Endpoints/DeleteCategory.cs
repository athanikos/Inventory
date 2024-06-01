
namespace Category.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;

    public class DeleteCategory :
        Endpoint<DeleteCategoryRequest>
    {
        private readonly IMediator _mediator;

        public  DeleteCategory(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Delete("/Category");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok<CategoryDto>, NotFound, ProblemDetails>>
            HandleAsync(DeleteCategoryRequest req,
                        CancellationToken ct)
        {
            var command = new DeleteCategoryCommand(
                req.Description);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<CategoryDto>(result);
        }
    }


    public record DeleteCategoryRequest(string Description);

    public record DeleteCategoryCommand(string Description)
      : IRequest<CategoryDto>;

  
}
