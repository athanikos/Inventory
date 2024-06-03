using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Entities = Inventory.Products.Entities;
using Inventory.Products;


namespace Inventory.Products.Handlers
{ 
    internal class DeleteCategoryHandler :
        IRequestHandler<DeleteCategoryCommand, CategoryDto>
    {
        private readonly ProductsDbContext  _context;

        public DeleteCategoryHandler(Inventory.Products.ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto> Handle(DeleteCategoryCommand request,
            CancellationToken cancellationToken)
        {
            Entities.Category prd =
                new Entities.Category()
            {
                Id = request.Id
          
            };

            _context.Categories.Add(prd);
            await _context.SaveChangesAsync(cancellationToken);

            return new CategoryDto(prd.Id,  prd.Name, prd.FatherId);
        }
    }
}
