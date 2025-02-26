
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Dto;
    using Inventory.Products.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class EditInventory(IInventoryRepository repo) :
        Endpoint<EditInventoryRequest>
    {
        public override void Configure()
        {
            Put("/inventories");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok<InventoryDto>, NotFound, ProblemDetails>>
            HandleAsync(EditInventoryRequest req,
                        CancellationToken ct)
        {
            var dto =  await repo.EditInventoryAsync(new InventoryDto( req.Id , req.Description, req.Code));
            return TypedResults.Ok(dto);
        }
    }


    public record EditInventoryRequest(Guid Id,  string Description, string Code );

   
  
}
