
namespace Product.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;

    public class EditCategory :
        Endpoint<EditCategoryRequest>
    {
        private readonly IMediator _mediator;

        public  EditCategory(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("/Category");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok<CategoryDto>, NotFound, ProblemDetails>>
            HandleAsync(EditCategoryRequest req,
                        CancellationToken ct)
        {
            var command = new EditCategoryCommand(
                req.Description);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<CategoryDto>(result);
        }
    }


    public record EditCategoryRequest(string Description);

    public record EditCategoryCommand(string Description)
      : IRequest<CategoryDto>;

  
}
