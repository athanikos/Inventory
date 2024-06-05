
namespace Inventory.Products.Endpoints
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

    public  class AddProduct 
        : Endpoint<AddProductRequest>
    {
        private readonly IInventoryRepository _repo;

        public  AddProduct(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Post("/product");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }


        public override async Task<Results<Ok<ProductDto>, NotFound, ProblemDetails>>
            HandleAsync(AddProductRequest req,
                        CancellationToken ct)
        {
           var dto =  await _repo.AddProductAsync(new ProductDto(req.ProductId, req.Description, req.InventoryId));
           return TypedResults.Ok(dto);
        }
    }
    public record AddProductRequest
    (Guid ProductId, string Description, Guid InventoryId);

    public record AddProductCommand(Guid ProductId, string Description, Guid InventoryId)
      : IRequest<ProductDto>;

  
}
