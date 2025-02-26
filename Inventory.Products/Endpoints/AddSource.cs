
namespace Source.Products.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;
    using System;
    using Inventory.Products.Repositories;

    public class AddSource(IInventoryRepository repository) :
        Endpoint<AddSourceRequest>
    {
        public override void Configure()
        {
            Post("/sources");
            AllowAnonymous();
            // to do claims this is per SourceId claim
            //  something like Admin_<SourceId>
        }

        public override async Task<Results<Ok<SourceDto>, NotFound, ProblemDetails>>
            HandleAsync(AddSourceRequest req,
                        CancellationToken ct)
        {
                var dto = new SourceDto(Guid.NewGuid(), req.Description);
                await repository.AddSourceAsync(dto);
                return TypedResults.Ok(dto);
        }
    }


    public record AddSourceRequest(Guid FatherId, string Description);

 
  
}
