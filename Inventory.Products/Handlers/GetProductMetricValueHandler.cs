using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Repositories;
using MediatR;

namespace Inventory.Products.Handlers
{
    //todo pass service
    
    public class GetProductMetricValueHandler(IInventoryRepository repo) :
        IRequestHandler<GetProductMetricQuery, ProductMetricDto>
    {
        public async  Task<ProductMetricDto> Handle(GetProductMetricQuery request, 
            CancellationToken cancellationToken)
        {
            return await  repo.GetProductMetricAsync(request.ProductCode,request.MetricCode);
        }

       
    }
}
