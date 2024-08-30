
namespace Transaction.Transactions.Endpoints
{
    using FastEndpoints;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;


    public class EditTransaction :
        Endpoint<DeleteTransactionRequest>
    {
        private readonly ITransactionRepository _repo;

        public  EditTransaction(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Delete("/transaction");
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteTransactionRequest req,
                        CancellationToken ct)
        {
            //todo fix optional parameters , just id contructor?
            await _repo.DeleteTransactionAsync(new TransactionDto(req.Id));
            return TypedResults.Ok();
        }
    }


    public record DeleteTransactionRequest(Guid Id);

  
  
}
