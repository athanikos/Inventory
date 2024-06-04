using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;

namespace Inventory.Products.Handlers
{
    internal class EditInventoryHandler :
        IRequestHandler<AddInventoryCommand, InventoryDto>
    {
        private readonly IInventoryRepository _repo;

        public EditInventoryHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<InventoryDto> Handle
            (AddInventoryCommand request, 
            CancellationToken cancellationToken)
        {
            return await _repo.EditInventoryAsync(new InventoryDto(request.InventoryId, request.Description)); 
        }

    
    }
}
