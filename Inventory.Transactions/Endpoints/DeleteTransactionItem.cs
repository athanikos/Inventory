using FastEndpoints;
using Inventory.Transactions.Dto;
using Inventory.Transactions.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TransactionItem.Transactions.Endpoints
{
    public class DeleteTransactionItem :
        Endpoint<DeleteTransactionItemRequest>
    {
        private readonly ITransactionRepository _repo;

        public DeleteTransactionItem(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Delete("/transactionitem");
            // to do claims this is per TransactionItemId claim
            //  something like Admin_<TransactionItemId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteTransactionItemRequest req,
                        CancellationToken ct)
        {
            await _repo.DeleteTransactionItemAsync(new TransactionItemDto(req.Id, Guid.Empty));

            return TypedResults.Ok();
        }
    }


    public record DeleteTransactionItemRequest(
                    Guid Id);

  
  
}
