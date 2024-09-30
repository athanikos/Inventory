
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteInventory(IInventoryRepository repo) :
        Endpoint<DeleteInventoryRequest>
    {
        public override void Configure()
        {
            Delete("/inventory");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteInventoryRequest req,
                        CancellationToken ct)
        {

            await repo.DeleteInventoryAsync(new Dto.InventoryDto(req.Id, string.Empty, string.Empty));
            return TypedResults.Ok(); //todo fix 
        }
    }


    public record DeleteInventoryRequest(Guid Id);

  
  
}
