namespace Inventory.Products.Contracts
{

    public class InventoryMetricDto(
        Guid inventoryId,
        Guid metricId,
        decimal value,
        DateTime effectiveDate,
        string inventoryCode,
        string metricCode)
    {
        public Guid InventoryId { get; } = inventoryId;

        public Guid MetricId { get; } = metricId;

        public string InventoryCode { get; set; } = inventoryCode;

        public string MetricCode { get; set; } = metricCode;

        public decimal Value { get; } = value;

        public DateTime EffectiveDate { get; } = effectiveDate;

     
        public Guid UnitOfMeasurementId { get; set; }

        
        public Guid SourceId { get; set; }

    }

}