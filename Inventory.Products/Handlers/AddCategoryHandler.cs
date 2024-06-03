using MediatR;
using Inventory.Products;
using Inventory.Products.Dto;
using Category.Products.Endpoints;

namespace Inventory.Categorys.Handlers
{
    internal class AddCategoryHandler : IRequestHandler<AddCategoryCommand, CategoryDto>
    {
        private readonly Products.ProductsDbContext  _context;

        public AddCategoryHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto> Handle(AddCategoryCommand request,
            CancellationToken cancellationToken)
        {
            Products.Entities.Category prd = new Products.Entities.Category()
            {
                Id = request.Id,
                FatherId = request.FatherId,
                Name = request.Description
            };

            _context.Categories.Add(prd);
            await _context.SaveChangesAsync(cancellationToken);

            return new CategoryDto(prd.Id,  prd.Name, prd.FatherId);
        }
    }
}
