using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;

namespace Inventory.Products.Handlers
{
    internal class EditProductHandler :
        IRequestHandler<EditProductCommand, ProductDto>
    {
        private readonly IInventoryRepository _repo;

        public EditProductHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<ProductDto> Handle
            (EditProductCommand request, 
            CancellationToken cancellationToken)
        {
            return await _repo.EditProductAsync(new ProductDto(request.id, request.Description, request.InventoryId));  
        }

    
    }
}
