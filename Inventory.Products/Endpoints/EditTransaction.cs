
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;
    using Azure.Core;
    using Microsoft.EntityFrameworkCore.Storage;
    using Inventory.Products.Repositories;

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
            var dto =  await _repo.AddTransactionAsync(new TransactionDto(req.Id, req.Description, req.Created));
            return TypedResults.Ok(dto);
        }
    }

    public record EditTransactionRequest(Guid Id,string Description, DateTime Created );

  
}
