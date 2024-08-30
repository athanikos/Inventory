
namespace Inventory.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Transactions.Repositories;
    using Inventory.Transactions.Dto;

    public class EditTransaction :
        Endpoint<EditTransactionRequest>
    {
        private readonly ITransactionRepository _repo;

        public EditTransaction(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Put("/transaction");
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TransactionDto>, NotFound, ProblemDetails>>
            HandleAsync(EditTransactionRequest req,
                        CancellationToken ct)
        {
            var dto =  await _repo.EditTransactionAsync(new TransactionDto(req.Id, req.Description,
                req.Created,req.TemplateId, req.Sections));
            return TypedResults.Ok(dto);
        }
    }

    public record EditTransactionRequest(Guid Id,string Description, DateTime Created, Guid TemplateId,
        ICollection<TransactionSectionDto> Sections );

  
}
