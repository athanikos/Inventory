namespace Inventory.Products.Contracts
{

    public class InventoryMetricDto(
        Guid inventoryId,
        Guid metricId,
        decimal value,
        DateTime effectiveDate,
        string currency,
        string inventoryCode,
        string metricCode)
    {
        public Guid InventoryId { get; } = inventoryId;

        public Guid MetricId { get; } = metricId;

        public string InventoryCode { get; set; } = inventoryCode;

        public string MetricCode { get; set; } = metricCode;

        public decimal Value { get; } = value;

        public DateTime EffectiveDate { get; } = effectiveDate;

        public string Currency { get; } = currency;

        public Guid SourceId { get; set; }

    }

}