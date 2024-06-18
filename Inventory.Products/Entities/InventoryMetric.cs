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
    }
}
