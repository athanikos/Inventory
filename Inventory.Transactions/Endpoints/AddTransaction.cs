﻿using FastEndpoints;
using Inventory.Transactions.Dto;
using Inventory.Transactions.Repositories;
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
            Post("/transaction");
            AllowAnonymous();
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TransactionDto>, NotFound, ProblemDetails>>
            HandleAsync(AddTransactionRequest req,
                        CancellationToken ct)
        {
            var dto = await service.UpdateOrInsertTransaction(
                new TransactionDto(req.TransactionId,
                                   req.Description,
                                   DateTime.Now,
                                   req.TemplateId,
                                   req.Sections));

            return TypedResults.Ok<TransactionDto>(dto);
        }
    }
    public record AddTransactionRequest(Guid TransactionId, 
        string Description,Guid TemplateId, ICollection<TransactionSectionDto> Sections );

  
}
