using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
        public  record GetProductMetricQuery(
                       Guid    InventoryId,
                       string   ProductCode,
                       string   MetricCode, 
                       DateTime EffectiveDate)   :  IRequest<ProductMetricDto>;
}
