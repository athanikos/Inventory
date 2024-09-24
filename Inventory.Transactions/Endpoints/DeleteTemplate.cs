
namespace Transaction.Transactions.Endpoints
{
    using FastEndpoints;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;


    public class DeleteTemplate :
        Endpoint<DeleteTemplateRequest>
    {
        private readonly ITransactionRepository _repo;

        public DeleteTemplate(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Delete("/template");
            AllowAnonymous();
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteTemplateRequest req,
                        CancellationToken ct)
        {
            await _repo.DeleteTemplateAsync(new TemplateDto(req.Id));
            return TypedResults.Ok();
        }
    }


    public record DeleteTemplateRequest(Guid Id);

  
  
}
