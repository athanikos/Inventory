﻿
namespace Inventory.Metrics.Endpoints
{
    using Azure.Core;
    using FastEndpoints;
    using Inventory.Products.Dto;
    using Inventory.Products.Repositories;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public  class DeleteMetric 
        : Endpoint<DeleteMetricRequest>
    {
        private readonly IInventoryRepository _repo;

        public  DeleteMetric(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Delete("/metric");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteMetricRequest req,
                        CancellationToken ct)
        {
            //todo default 
            await _repo.DeleteMetricAsync(new MetricDto(req.Id, string.Empty, 0, DateTime.MinValue, string.Empty, Guid.Empty));
            return TypedResults.Ok();
        }
    }


    public record DeleteMetricRequest (Guid Id);

    public record DeleteMetricCommand (Guid Id) : IRequest;

  
}
