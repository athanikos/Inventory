using MediatR;
using Transaction.Products.Endpoints;

namespace Inventory.Products.Handlers
{
    internal class DeleteTransactionHandler   :
        IRequestHandler<DeleteTransactionCommand>
    {
        private readonly ProductsDbContext _context;

        public DeleteTransactionHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task Handle
            (DeleteTransactionCommand request, 
            CancellationToken cancellationToken)
        {
            var itemToRemove = _context.Transactions.SingleOrDefault(x => x.Id == request.Id);
            if (itemToRemove != null)
            {
                _context.Transactions.Remove(itemToRemove);
                await _context.SaveChangesAsync(cancellationToken);
            }

        }

    
    }
}
