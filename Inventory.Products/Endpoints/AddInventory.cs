using Inventory.Products.Services;
using Common;

namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Dto;
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
            AllowAnonymous(); // todo remove
                              // to do claims this is per InventoryId claim
                              //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok<InventoryDto>, NotFound, ProblemDetails>>
            HandleAsync(AddInventoryRequest req,
                        CancellationToken ct)
        {
            return await new EndPointHandleWrapper<InventoryDto, AddInventoryRequest,
                               CancellationToken>(Handle, req, ct).Execute();
        }

        private async Task<InventoryDto>
            Handle(AddInventoryRequest req, CancellationToken ct)
        {
            return await _service.AddInventoryAsync(req.ToInventoryDto());
        }
    }

    public record AddInventoryRequest(Guid InventoryId, string Description, string Code);

    public static class AddInventoryRequestExtensions
    {
        public static InventoryDto ToInventoryDto(this AddInventoryRequest request)
        {
            return new InventoryDto(request.InventoryId, request.Description, request.Code);
        }
    }
}
