﻿
namespace Transaction.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;
    using Azure.Core;
    using Inventory.Products.Repositories;

    public class EditTransaction :
        Endpoint<DeleteTransactionRequest>
    {
        private readonly ITransactionRepository _repo;

        public  EditTransaction(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Delete("/transaction");
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteTransactionRequest req,
                        CancellationToken ct)
        {
            await _repo.DeleteTransactionAsync(new TransactionDto(req.Id, string.Empty, DateTime.MinValue));
            return TypedResults.Ok();
        }
    }


    public record DeleteTransactionRequest(Guid Id);

    public record DeleteTransactionCommand(Guid Id)
      : IRequest;

  
}
