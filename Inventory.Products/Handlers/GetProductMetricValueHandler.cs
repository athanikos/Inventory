using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Repositories;
using MediatR;

namespace Inventory.Products.Handlers
{
    public class GetProductMetricValueHandler : 
        IRequestHandler<GetProductMetricValueQuery, ProductMetricDto>
    {

        private readonly IInventoryRepository _repo;

        public GetProductMetricValueHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async  Task<ProductMetricDto> Handle(GetProductMetricValueQuery request, 
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return _repo.GetProductMetric(request.ProductCode, request.MetricCode);
        }

       
    }
}
