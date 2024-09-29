using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
        public  record GetProductMetricQuery(string   ProductCode, 
                string   MetricCode)   :  IRequest<ProductMetricDto>;
}

