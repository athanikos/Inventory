using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Entities = Inventory.Products.Entities;
using Inventory.Products.Repositories;

namespace Inventory.Products.Handlers
{
    internal class DeleteProductHandler :
        IRequestHandler<DeleteProductCommand>
    {
        private readonly IInventoryRepository _repo;

        public DeleteProductHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle
            (DeleteProductCommand request, 
            CancellationToken cancellationToken)
        {
           await  _repo.DeleteProductAsync(new ProductDto(request.Id,string.Empty,Guid.Empty));
        }

    
    }
}
