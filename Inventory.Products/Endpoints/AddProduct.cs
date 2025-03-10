﻿
using Inventory.Products.Contracts;

namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Contracts.Dto;
    using Inventory.Products.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddProduct(IInventoryRepository repo) : Endpoint<AddProductRequest>
    {
        public override void Configure()
        {
            Post("/products");
            AllowAnonymous(); // todo remove
                              // to do claims this is per InventoryId claim
                              //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok<ProductDto>, NotFound, ProblemDetails>>
            HandleAsync(AddProductRequest req,
                        CancellationToken ct)
        {
            var dto = new ProductDto(req.ProductId, req.Description, req.Code, req.InventoryId, req.Metrics);

            if (!repo.InventoryIdExists(req.InventoryId))
            {
                AddError("Inventory Id does not exist ");
                ThrowIfAnyErrors(); // If there are errors, execution shouldn't go beyond this point
                return new ProblemDetails(ValidationFailures);
            }

            if (await repo.ProductDescriptionOrCategoryIsUsedAsync(dto))
            {
                AddError("Product code or descr is used  ");
                ThrowIfAnyErrors(); // If there are errors, execution shouldn't go beyond this point
                return new ProblemDetails(ValidationFailures);
            }

            dto = await repo.AddProductAsync(dto);
            return TypedResults.Ok(dto);
        }
    }
    public record AddProductRequest(Guid ProductId, string Description, string Code, Guid InventoryId, List<ProductMetricDto> Metrics);

    
}
