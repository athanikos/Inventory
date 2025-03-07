using FastEndpoints;
using Inventory.Transactions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inventory.Transactions.Endpoints
{
    public class TemplatesDataPrepare(ITransactionService service) :
        Endpoint<DataPrepareRequest>
    {
        public override void Configure()
        {
            Post("/TemplatesDataPrepare");
            AllowAnonymous();
            // to do claims this is per EntityId claim
            //  something like Admin_<EntityId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DataPrepareRequest edbr, CancellationToken ct)
        {

             await service.RoomsPrepareAsync();
             return TypedResults.Ok();
        }
    }
  
    public record DataPrepareRequest(string Name) { }
}
