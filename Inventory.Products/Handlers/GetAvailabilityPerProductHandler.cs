using Inventory.Products.Contracts.Dto;
using Inventory.Products.Repositories;
using MediatR;

namespace Inventory.Products.Handlers;

public class GetAvailabilityPerProductHandler(IInventoryRepository repository) :
    IRequestHandler<GetAvailabilityPerProductCommand,
        List<ModifyQuantityDto>>
{
    private readonly IInventoryRepository _repository = repository;

    public Task<List<ModifyQuantityDto>> Handle(GetAvailabilityPerProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}


public abstract class GetAvailabilityPerProductCommand(Guid inventoryId) 
    : IRequest<List<ModifyQuantityDto>>
{
    public Guid InventoryId { get; } = inventoryId;
}