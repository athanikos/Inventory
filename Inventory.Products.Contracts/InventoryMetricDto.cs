namespace Inventory.Products.Contracts.Dto
{

    public class InventoryMetricDto
    {
        public InventoryMetricDto(
            Guid inventoryId,
            Guid metricId,
            decimal value,
            DateTime effectiveDate,
            string currency,
            string inventoryCode,
            string metricCode
            )
        {
            InventoryId = inventoryId;
            MetricId = metricId;
            Value = value;
            EffectiveDate = effectiveDate;
            Currency = currency;
            InventoryCode = inventoryCode;
            MetricCode = metricCode;
        }

        public Guid InventoryId { get; set; }

        public Guid MetricId { get; set; }

        public string InventoryCode { get; set; } = string.Empty;

        public string MetricCode { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public DateTime EffectiveDate { get; set; }
        = DateTime.MinValue;

        public string Currency { get; set; } = string.Empty;

        public Guid SourceId { get; set; }

    }

}