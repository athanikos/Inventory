using FastEndpoints;
using Inventory.Transactions.Dto;
using Inventory.Transactions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inventory.Transactions.Endpoints
{
    public class DeleteTransaction(ITransactionService service) :
        Endpoint<DeleteTransactionRequest>
    {
        public override void Configure()
        {
            Delete("/transactions");
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteTransactionRequest req,
                        CancellationToken ct)
        {
            await service.DeleteTransactionAsync(new TransactionDto(req.Id));
            return TypedResults.Ok();
        }
    }


    public record DeleteTransactionRequest(Guid Id);

  
  
}
