
namespace Transaction.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Transactions.Repositories;
    using Microsoft.AspNetCore.Http;

    public class EmptyDB :
        Endpoint<EmptyDBRequest>
    {
        private readonly ITransactionRepository _repo;
        public EmptyDB(ITransactionRepository repo)  {  _repo = repo;  }

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
            await _repo.EmptyDB();
            return TypedResults.Ok();
        }
    }
  
    public record EmptyDBRequest(string Name) { }
}
