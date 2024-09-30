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

    public class InitializeConfig(IMediator mediator, IInventoryService service) :
        Endpoint<InitializeConfigRequest>
    {
        private readonly IMediator _mediator = mediator;

        public override void Configure()
        {
            Post("/Configuration");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(InitializeConfigRequest req,
                        CancellationToken ct)
        {
            try
            {
                  await service.InitialConfigureAsync();
                  return TypedResults.Ok();
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                ThrowIfAnyErrors(); // If there are errors, execution shouldn't go beyond this point
                return new FastEndpoints.ProblemDetails(ValidationFailures);
            }

        }
    }


    public  record InitializeConfigRequest();

 
  
}
