using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;

namespace Inventory.Products.Handlers
{
    internal class AddInventoryHandler :
        IRequestHandler<AddInventoryCommand, InventoryDto>
    {
        private readonly IInventoryRepository _repo;

        public AddInventoryHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<InventoryDto> Handle
            (AddInventoryCommand request, 
            CancellationToken cancellationToken)
        {
            return await _repo.AddInventoryAsync(
                new InventoryDto(request.InventoryId, request.Description));

        }

    
    }
}
