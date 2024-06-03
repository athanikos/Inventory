using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Entities = Inventory.Products.Entities;

namespace Inventory.Products.Handlers
{
    internal class DeleteProductHandler :
        IRequestHandler<DeleteProductCommand>
    {
        private readonly Inventory.Products.ProductsDbContext _context;

        public DeleteProductHandler(Inventory.Products.ProductsDbContext context)
        {
            _context = context;
        }

        public async Task Handle
            (DeleteProductCommand request, 
            CancellationToken cancellationToken)
        {
            var itemToRemove = _context.Products.
                SingleOrDefault(x => x.Id == request.Id);
            if (itemToRemove != null)
            {
                _context.Products.Remove(itemToRemove);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

    
    }
}
