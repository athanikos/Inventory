
using Inventory.Transactions.Services;

namespace Inventory.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Dto;

    public class EditTransaction(ITransactionService service) :
        Endpoint<EditTransactionRequest>
    {
        public override void Configure()
        {
            Put("/transactions");
            AllowAnonymous();
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TransactionDto>, NotFound, ProblemDetails>>
            HandleAsync(EditTransactionRequest req,
                        CancellationToken ct)
        {

            var dto = await service.UpdateOrInsertTransaction(
                new TransactionDto(req.Id,
                    req.Description,
                    DateTime.Now,
                    req.TemplateId,
                    req.Sections));

            return TypedResults.Ok(dto);
        }
    }

    public record EditTransactionRequest(Guid Id,string Description, DateTime Created, Guid TemplateId,
        ICollection<TransactionSectionDto> Sections );

  
}
