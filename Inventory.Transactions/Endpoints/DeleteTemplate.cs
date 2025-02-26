using FastEndpoints;
using Inventory.Transactions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inventory.Transactions.Endpoints
{
    public class DeleteTemplate(ITransactionService service) :
        Endpoint<DeleteTemplateRequest>
    {
        public override void Configure()
        {
            Delete("/templates");
            AllowAnonymous();
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteTemplateRequest req,
                        CancellationToken ct)
        {
            await service.DeleteTemplateAsync(new TemplateDto(req.Id));
            return TypedResults.Ok();
        }
    }


    public record DeleteTemplateRequest(Guid Id);

  
  
}
