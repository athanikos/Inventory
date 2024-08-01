
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Dto;
    using Inventory.Products.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddInventory :
        Endpoint<AddInventoryRequest>
    {
        private readonly IInventoryRepository _repo;

        public  AddInventory(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Post("/inventory");
            AllowAnonymous();
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok<InventoryDto>, NotFound, ProblemDetails>>
            HandleAsync(AddInventoryRequest req,
                        CancellationToken ct)
        {
            var dto =  await _repo.AddInventoryAsync(
            new InventoryDto(req.InventoryId, req.Description));
            return TypedResults.Ok( dto    );
        }
    }


    public record AddInventoryRequest(Guid InventoryId, string Description);

 
  
}
