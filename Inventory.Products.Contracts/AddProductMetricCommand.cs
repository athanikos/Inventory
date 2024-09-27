
using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
    public class AddProductMetricCommand(
        Guid productId,
        Guid metricId,
        decimal value,
        DateTime effectiveDate,
        Guid unitOfMeasurementId)
        : IRequest<ProductMetricDto>
    {
        public Guid ProductId { get; set; } = productId;

        public Guid MetricId { get; set; } = metricId;

        public decimal Value { get; set; } = value;

        public DateTime EffectiveDate { get; set; } = effectiveDate;

     
        public Guid UnitOfMeasurementId { get; set; } = unitOfMeasurementId;
    }
}
