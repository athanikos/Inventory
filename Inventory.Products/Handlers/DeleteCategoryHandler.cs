using MediatR;
using Inventory.Products.Dto;
using Category.Products.Endpoints;


namespace Inventory.Products.Handlers
{
    internal class DeleteCategoryHandler :
        IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ProductsDbContext  _context;

        public DeleteCategoryHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var itemToRemove = _context.Categories.SingleOrDefault(x => x.Id == request.Id);
            if (itemToRemove != null)
            {
                _context.Categories.Remove(itemToRemove);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
