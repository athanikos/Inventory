
namespace Inventory.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Transactions.Repositories;
    using Inventory.Transactions.Dto;

    public class EditTemplate :
        Endpoint<EditTemplateRequest>
    {
        private readonly ITransactionRepository _repo;

        public EditTemplate(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Put("/template");
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TemplateDto>, NotFound, ProblemDetails>>
            HandleAsync(EditTemplateRequest req,
                        CancellationToken ct)
        {
            return TypedResults.Ok(
                   await _repo.EditTemplateAsync(new TemplateDto(req.Id, req.Name,req.Type,req.Created,req.Fields)));
            
        }
    }

    public record EditTemplateRequest(Guid Id,string Name, DateTime Created, TemplateType Type,
                             ICollection<FieldDto> Fields);

  
}
