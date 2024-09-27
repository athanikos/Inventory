using Inventory.Products.Contracts;

namespace Inventory.Products.Entities
{
    public  class InventoryMetric
    {
        public Guid InventoryId  { get; set; }

        public Guid MetricId { get; set; }

        public string InventoryCode { get; set; } = string.Empty;

        public string MetricCode { get; set; } = string.Empty;

        public decimal Value { get; internal set; }

        public Guid UnitOfMeasurementId { get; set; }

        public DateTime EffectiveDate { get; internal set; }
        = DateTime.MinValue;

        public static InventoryMetric CreateInventoryMetric(InventoryMetricDto m)
        {
            return new InventoryMetric()
            {
                MetricId = m.MetricId,
                EffectiveDate = m.EffectiveDate,
                InventoryId = m.InventoryId,
                Value = m.Value, 
                InventoryCode = m.InventoryCode,
                MetricCode = m.MetricCode,
                UnitOfMeasurementId = m.UnitOfMeasurementId
 
            };
        }

    }
}
