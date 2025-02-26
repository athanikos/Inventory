﻿
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Contracts.Dto;
    using Inventory.Products.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddQuantityMetric(IInventoryRepository repo) :
        Endpoint<AddQuantityMetricRequest>
    {
        public override void Configure()
        {
            Post("/quantitymetrics");
            AllowAnonymous();//todo remove 
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok<QuantityMetricDto>, NotFound, ProblemDetails>>
            HandleAsync(AddQuantityMetricRequest req,
                        CancellationToken ct)
        {
            var dto = await repo.AddQuantityMetricAsync(
                                  new QuantityMetricDto(req.ProductId, req.Value,
                                                       req.EffectiveDate, req.transactionId, req.diff, req.isCancelled));
            return TypedResults.Ok(dto);
        }  
    }


    public record AddQuantityMetricRequest(Guid ProductId, 
         decimal Value, DateTime EffectiveDate, Guid transactionId,
            decimal diff,
            bool isCancelled);



}
