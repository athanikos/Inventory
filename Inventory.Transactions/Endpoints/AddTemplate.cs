
namespace Transaction.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;
    using Microsoft.AspNetCore.Http;

    public class AddTemplate :
        Endpoint<AddTemplateRequest>
    {
        private readonly ITransactionRepository _repo;

        public AddTemplate(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Post("/transactionItemTemplate");
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TemplateDto>, NotFound, ProblemDetails>>
            HandleAsync(AddTemplateRequest req,
                        CancellationToken ct)
        {
            var dto = await _repo.AddTemplateAsync(
                new TemplateDto(Guid.Empty, req.Name, req.Type, DateTime.UtcNow, req.Fields));
                
            return TypedResults.Ok
                <TemplateDto>(dto);
        }
    }


   public record AddTemplateRequest(               string Name,
                                                   DateTime Created,
                                                   TemplateType Type,
                                                    ICollection<FieldDto> Fields


       ) ;

  
}
