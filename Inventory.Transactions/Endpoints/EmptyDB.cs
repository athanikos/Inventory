using FastEndpoints;
using Inventory.Transactions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inventory.Transactions.Endpoints
{
    public class EmptyDb(ITransactionService service ) :
        Endpoint<EmptyDBRequest>
    {
        public override void Configure()
        {
            Post("/transactionsEmptyDB");
            AllowAnonymous();
            // to do claims this is per EntityId claim
            //  something like Admin_<EntityId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(EmptyDBRequest edbr, CancellationToken ct)
        {
            await service.EmptyDb();
            return TypedResults.Ok();
        }
    }
  
    public record EmptyDBRequest(string Name) { }
}
