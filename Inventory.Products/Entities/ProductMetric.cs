namespace Inventory.Products.Entities;

public class ProductMetric
{
    public Guid ProductId { get; set; }

    public Guid MetricId { get; set; }

    public decimal Value { get; internal set; }

    public DateTime EffectiveDate { get; internal set; }
    = DateTime.MinValue;
}


