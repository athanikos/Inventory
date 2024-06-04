using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;


namespace Inventory.Products.Handlers
{
    internal class EditCategoryHandler :
        IRequestHandler<EditCategoryCommand, CategoryDto>
    {
        private readonly ProductsDbContext  _context;

        public EditCategoryHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto> Handle(EditCategoryCommand request,
            CancellationToken cancellationToken)
        {
            Entities.Category prd =
                new Entities.Category()
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
