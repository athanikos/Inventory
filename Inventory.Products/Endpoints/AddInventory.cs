

using Inventory.Products.Services;

namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Dto;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddInventory(IInventoryService service) :
        Endpoint<AddInventoryRequest>
    {
        private readonly IInventoryService _service = service;

        public override void Configure()
        {
            Post("/inventories");
            AllowAnonymous();//todo remove 
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok<InventoryDto>, NotFound, ProblemDetails>>
            HandleAsync(AddInventoryRequest req,
                        CancellationToken ct)
        {
          var dto = await _service.AddInventoryAsync(new InventoryDto(req.InventoryId, req.Description, req.Code));
          return TypedResults.Ok( dto    );
        }
    }


    public record AddInventoryRequest(Guid InventoryId, string Description,string Code );

 
  
}
