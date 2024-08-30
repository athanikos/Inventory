
namespace Transaction.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;
    using Microsoft.AspNetCore.Http;

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
            AllowAnonymous();
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TransactionDto>, NotFound, ProblemDetails>>
            HandleAsync(AddTransactionRequest req,
                        CancellationToken ct)
        {


            var dto = await _repo.AddTransactionAsync(
                new TransactionDto(req.TransactionId,
                                   req.Description,
                                   DateTime.Now,
                                   req.TemplateId,
                                   req.Sections));

            return TypedResults.Ok<TransactionDto>(dto);
        }
    }
    public record AddTransactionRequest(Guid TransactionId, 
        string Description,Guid TemplateId, ICollection<TransactionSectionDto> Sections );

  
}
