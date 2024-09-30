using FastEndpoints;
using Inventory.Defaults.Services;
using Inventory.Products.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inventory.Defaults.Endpoints
{
    
    public class InitializeConfig(IMediator mediator, 
        IConfigurationService service) :
        Endpoint<InitializeConfigRequest>
    {
        public override void Configure()
        {
            Post("/Configuration");
        }

        public override async Task<Results<
                Ok<List<InitializeConfigurationResponse>>,
                NotFound,ProblemDetails>>
            HandleAsync(InitializeConfigRequest req,
            CancellationToken ct)
        {
            try
            {
                var items =  await mediator.
                    Send(new InitializeConfigurationCommand(), ct);
                await service.SaveAsync(items);
                return TypedResults.Ok(items);
                
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                ThrowIfAnyErrors(); // If there are errors, execution shouldn't go beyond this point
                return new ProblemDetails(ValidationFailures);
            }

        }
    }


    public  record InitializeConfigRequest();

 
  
}
