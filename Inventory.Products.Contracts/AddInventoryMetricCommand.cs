using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
    public class AddInventoryMetricCommand(
        Guid inventoryId,
        Guid metricId,
        decimal value,
        DateTime effectiveDate
      )
        : IRequest<InventoryMetricDto>
    {
        public Guid InventoryId { get; set; } = inventoryId;

        public Guid MetricId { get; set; } = metricId;

        public decimal Value { get; set; } = value;

        public DateTime EffectiveDate { get; set; } = effectiveDate;

    }
}
