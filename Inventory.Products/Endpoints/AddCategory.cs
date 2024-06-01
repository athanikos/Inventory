
namespace Category.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;

    public class AddCategory :
        Endpoint<AddCategoryRequest>
    {
        private readonly IMediator _mediator;

        public  AddCategory(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/Category");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok<CategoryDto>, NotFound, ProblemDetails>>
            HandleAsync(AddCategoryRequest req,
                        CancellationToken ct)
        {
            var command = new AddCategoryCommand(
                req.Description);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<CategoryDto>(result);
        }
    }


    public record AddCategoryRequest(string Description);

    public record AddCategoryCommand(string Description)
      : IRequest<CategoryDto>;

  
}
