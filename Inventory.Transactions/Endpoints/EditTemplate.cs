
using Inventory.Transactions.Services;

namespace Inventory.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Dto;

    public class EditTemplate(ITransactionService service) :
        Endpoint<EditTemplateRequest>
    {
        public override void Configure()
        {
            Put("/templates");
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TemplateDto>, NotFound, ProblemDetails>>
            HandleAsync(EditTemplateRequest req,
                        CancellationToken ct)
        {
            return TypedResults.Ok(
                   await service.EditTemplateAsync(new TemplateDto(req.Id, req.Name,req.Type,DateTime.UtcNow,req.Sections)));
            
        }
    }

    public record EditTemplateRequest(Guid Id,string Name, TemplateType Type,
                             ICollection<SectionDto> Sections);

  
}
