using FastEndpoints;
using Inventory.Products.Dto;
using Inventory.Products.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Web.Resource;

namespace Inventory.Products.Endpoints
{
    [RequiredScope("products.read")]

    public class AddCategory(IMediator mediator, 
        IInventoryService service) :
        Endpoint<AddCategoryRequest>
    {
        private readonly IMediator _mediator = mediator;

        public override void Configure()
        {
            Post("/Category");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok<CategoryDto>, 
                NotFound, ProblemDetails>>
            HandleAsync(AddCategoryRequest req,
                        CancellationToken ct)
        {

            try
            {
                  var categoryDto =  await service.AddCategoryAsync(new CategoryDto(Guid.NewGuid(), req.Description, req.FatherId));
                  return TypedResults.Ok(categoryDto);
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                ThrowIfAnyErrors(); // If there are errors, execution shouldn't go beyond this point
                return new ProblemDetails(ValidationFailures);

            }

        }
    }


    public abstract record AddCategoryRequest(Guid FatherId, string Description);

 
  
}
