using Inventory.Products.Contracts.Dto;
using Inventory.Products.Repositories;
using MediatR;

namespace Inventory.Products.Handlers;


public class GetAvailabilityPerInventoryHandler(IInventoryRepository repository) :
    IRequestHandler<GetAvailabilityPerInventoryCommand,
        List<ModifyQuantityDto>>
{
    public Task<List<ModifyQuantityDto>> Handle(GetAvailabilityPerInventoryCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}


public abstract class GetAvailabilityPerInventoryCommand(Guid inventoryId) 
    : IRequest<List<ModifyQuantityDto>>
{
    public Guid InventoryId { get; } = inventoryId;
}


