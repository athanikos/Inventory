using MediatR;
using Inventory.Metrics.Endpoints;
using Inventory.Products.Repositories;


namespace Inventory.Products.Handlers
{
    internal class DeleteMetricHandler :
        IRequestHandler<DeleteMetricCommand>
    {
        private readonly IInventoryRepository _repo;

        public DeleteMetricHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task 
            Handle(DeleteMetricCommand request,
            CancellationToken cancellationToken)
        {
              await   _repo.DeleteMetricAsync(new Dto.MetricDto(request.Id, string.Empty, 0, DateTime.MinValue, string.Empty, Guid.Empty));
        }
    }
}
