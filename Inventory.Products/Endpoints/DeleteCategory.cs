
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
            Delete("/category");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteCategoryRequest req,
                        CancellationToken ct)
        {
            var command = new DeleteCategoryCommand(
                req.Id);
          
            await _mediator!.
                Send(command, ct);
           
            return TypedResults.Ok(); //todo fix 
        }
    }


    public record DeleteCategoryRequest(Guid Id);

    public record DeleteCategoryCommand(Guid Id)
      : IRequest<CategoryDto>;

  
}
