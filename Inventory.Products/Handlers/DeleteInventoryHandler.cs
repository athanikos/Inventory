using Inventory.Products.Endpoints;
using Inventory.Products.Repositories;
using MediatR;

namespace Inventory.Products.Handlers
{
    internal class DeleteInventoryHandler :
        IRequestHandler<DeleteInventoryCommand>
    {
        private readonly IInventoryRepository _repo;

        public DeleteInventoryHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle
            (DeleteInventoryCommand request, 
            CancellationToken cancellationToken)
        {
            await    _repo.DeleteInventoryAsync(new Dto.InventoryDto(request.Id,string.Empty));
        }

    
    }
}
