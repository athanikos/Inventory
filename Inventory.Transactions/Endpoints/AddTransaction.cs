using FastEndpoints;
using Inventory.Transactions.Dto;
using Inventory.Transactions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inventory.Transactions.Endpoints
{
    public class AddTransaction(ITransactionService service) :
        Endpoint<AddTransactionRequest>
    {
        public override void Configure()
        {
            Post("/transactions");
            AllowAnonymous();
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TransactionDto>, NotFound, ProblemDetails>>
            HandleAsync(AddTransactionRequest req,
                        CancellationToken ct)
        {
            var dto = await service.UpdateOrInsertTransaction(
                             AddTransactionRequestExtensions.ToDto(req));

            return TypedResults.Ok(dto);
        }

      
    }

    public record AddTransactionRequest(Guid TransactionId,
        string Description, Guid TemplateId, ICollection<TransactionSectionDto> Sections);


    public static class AddTransactionRequestExtensions
    {
        public static TransactionDto ToDto(this AddTransactionRequest req)
        {
            return new TransactionDto(req.TransactionId,
                                               req.Description,
                                               DateTime.Now,
                                               req.TemplateId,
                                               req.Sections);
        }
    }
}
