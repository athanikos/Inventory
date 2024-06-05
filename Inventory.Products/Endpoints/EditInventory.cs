
namespace Inventory.Products.Endpoints
{
    using Azure.Core;
    using FastEndpoints;
    using Inventory.Products.Dto;
    using Inventory.Products.Repositories;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.EntityFrameworkCore.Storage.Json;
    using System.Threading;
    using System.Threading.Tasks;

    public class EditInventory :
        Endpoint<EditInventoryRequest>
    {
        private readonly IInventoryRepository _repo;

        public  EditInventory(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Put("/inventory");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok<InventoryDto>, NotFound, ProblemDetails>>
            HandleAsync(EditInventoryRequest req,
                        CancellationToken ct)
        {
            var dto =  await _repo.EditInventoryAsync(new InventoryDto( req.Id , req.Description));
            return TypedResults.Ok(dto);
        }
    }


    public record EditInventoryRequest(Guid Id,  string Description);

    public record EditInventoryCommand(Guid Id, string Description)
      : IRequest<InventoryDto>;

  
}
