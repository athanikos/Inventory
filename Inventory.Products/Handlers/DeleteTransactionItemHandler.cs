using MediatR;
using TransactionItem.Products.Endpoints;

namespace Inventory.Products.Handlers
{
    internal class DeleteTransactionItemHandler :
        IRequestHandler<DeleteTransactionItemCommand>
    {
        private readonly ProductsDbContext _context;

        public DeleteTransactionItemHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task Handle
            (DeleteTransactionItemCommand request, 
            CancellationToken cancellationToken)
        {
    
            var itemToRemove = _context.TransactionItems.
                SingleOrDefault(x => x.Id == request.Id);
            if (itemToRemove != null)
            {
                _context.TransactionItems.Remove(itemToRemove);
                await _context.SaveChangesAsync(cancellationToken);
            }

        }

    
    }
}
