
namespace Inventory.Products.Endpoints
{
    using Azure.Core;
    using FastEndpoints;
    using Inventory.Products.Repositories;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteInventory :
        Endpoint<DeleteInventoryRequest>
    {
        private readonly IInventoryRepository _repo;

        public  DeleteInventory(IInventoryRepository repo)
        {
            _repo = repo;
        }

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

            await _repo.DeleteInventoryAsync(new Dto.InventoryDto(req.Id, string.Empty));
            return TypedResults.Ok(); //todo fix 
        }
    }


    public record DeleteInventoryRequest(Guid Id);

    public record DeleteInventoryCommand(Guid Id): IRequest;

  
}
