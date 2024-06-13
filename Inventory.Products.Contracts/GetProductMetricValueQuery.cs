using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
    public  record GetProductMetricValueQuery
        (string ProductCode, string MetricCode, DateTime EffectiveDate ) :  IRequest<ProductMetricDto>;






}
