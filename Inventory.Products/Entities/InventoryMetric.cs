using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Entities
{
    public  class InventoryMetric
    {
        public Guid InventoryId  { get; set; }

        public Guid MetricId { get; set; }

        public string InventoryCode { get; set; } = string.Empty;

        public string MetricCode { get; set; } = string.Empty;

        public decimal Value { get; internal set; }

        public string Currency { get; set; } = string.Empty;

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
                Currency = m.Currency,
                InventoryCode = m.InventoryCode,
                MetricCode = m.MetricCode
            };
        }

    }
}
