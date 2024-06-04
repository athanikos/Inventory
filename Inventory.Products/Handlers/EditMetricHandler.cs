using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;


namespace Inventory.Products.Handlers
{
    public class EditMetricHandler :
        IRequestHandler<EditMetricCommand, MetricDto>
    {
        private readonly IInventoryRepository _repo;

        public EditMetricHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<MetricDto> Handle(EditMetricCommand request,
            CancellationToken cancellationToken)
        {
            return await _repo.EditMetricAsync(new MetricDto(request.Id, request.Description, request.Value, request.EffectiveDate, request.Code, request.SourceId));
        }
    }
}
