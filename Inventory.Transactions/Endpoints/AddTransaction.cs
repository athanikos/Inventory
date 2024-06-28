
namespace Transaction.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;

    public class AddTransaction :
        Endpoint<AddTransactionRequest>
    {
        private readonly ITransactionRepository _repo;

        public  AddTransaction(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Post("/transaction");
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TransactionDto>, NotFound, ProblemDetails>>
            HandleAsync(AddTransactionRequest req,
                        CancellationToken ct)
        {

            throw new NotImplementedException();
            //var dto =  await _repo.AddTransactionAsync(
            //    new TransactionDto(req.TransactionId, 
            //                       req.Description, 
            //                       req.Created));
        
            //return TypedResults.Ok<TransactionDto>(dto);
        }
    }
    public record AddTransactionRequest(Guid TransactionId, string Description, DateTime Created);

  
}
