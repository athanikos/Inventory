using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;

namespace Inventory.Products.Handlers
{
    internal class AddProductHandler : IRequestHandler<AddProductCommand, ProductDto>
    {
        private readonly IInventoryRepository _repo;

        public AddProductHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<ProductDto> Handle(AddProductCommand request,
            CancellationToken cancellationToken)
        {
            return await _repo.AddProductAsync(new ProductDto(request.ProductId, request.Description, request.InventoryId));
        }
    }
}
