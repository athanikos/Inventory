using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;

namespace Inventory.Products.Handlers
{
    internal class AddProductHandler : IRequestHandler<AddProductCommand, ProductDto>
    {
        private readonly ProductsDbContext _context;

        public AddProductHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> Handle(AddProductCommand request,
            CancellationToken cancellationToken)
        {
            Entities.Product prd = new Entities.Product()
            {
                Description = request.Description,
                InventoryId = request.InventoryId
            };

            _context.Products.Add(prd);
            await _context.SaveChangesAsync(cancellationToken);

            return new ProductDto(prd.Id, prd.Description);
        }
    }
}
