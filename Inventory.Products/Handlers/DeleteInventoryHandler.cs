using Inventory.Products.Endpoints;
using MediatR;

namespace Inventory.Products.Handlers
{
    internal class DeleteInventoryHandler :
        IRequestHandler<DeleteInventoryCommand>
    {
        private readonly ProductsDbContext _context;

        public DeleteInventoryHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task Handle
            (DeleteInventoryCommand request, 
            CancellationToken cancellationToken)
        {
       
            var itemToRemove = _context.Inventories.SingleOrDefault(x => x.Id == request.Id);
            if (itemToRemove != null)
            {
                _context.Inventories.Remove(itemToRemove);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

    
    }
}
