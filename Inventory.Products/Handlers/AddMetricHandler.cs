using Inventory.Metrics.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;

namespace Inventory.Metrics.Handlers
{
    internal class AddMetricHandler : IRequestHandler<AddMetricCommand, MetricDto>
    {
        private readonly IInventoryRepository _repo;

        public AddMetricHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<MetricDto> Handle(AddMetricCommand request,
            CancellationToken cancellationToken)
        {
 
           return await _repo.AddMetricAsync(new MetricDto(request.Id, request.Description, request.Value, request.EffectiveDate,
                request.Code, request.SourceId));
        }
    }
}
