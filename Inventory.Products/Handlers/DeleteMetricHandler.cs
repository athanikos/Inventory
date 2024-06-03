using MediatR;
using Inventory.Metrics.Endpoints;


namespace Inventory.Products.Handlers
{
    internal class DeleteMetricHandler :
        IRequestHandler<DeleteMetricCommand>
    {
        private readonly ProductsDbContext  _context;

        public DeleteMetricHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task 
            Handle(DeleteMetricCommand request,
            CancellationToken cancellationToken)
        {
            var itemToRemove = _context.Metrics.SingleOrDefault(x => x.Id == request.Id);
            if (itemToRemove != null)
            {
                _context.Metrics.Remove(itemToRemove);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
