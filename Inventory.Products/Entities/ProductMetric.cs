using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Entities;

/// <summary>
/// Customizable product metrics using metric and productId and effective date as PK 
/// 
/// </summary>
public class ProductMetric
{
    public Guid ProductId { get; set; }

    public Guid MetricId { get; set; }

    public string ProductCode { get; set; } = string.Empty;

    public string MetricCode { get; set;  } = string.Empty;

    public decimal Value { get; internal set; }

    public string Currency { get; set; } = string.Empty;

    public DateTime EffectiveDate { get; internal set; }  = DateTime.MinValue;

    public static ProductMetric CreateProductMetric(ProductMetricDto m)
    {
        return new ProductMetric()
        {
            MetricId = m.MetricId,
            EffectiveDate = m.EffectiveDate,
            ProductId = m.ProductId,
            Value = m.Value,
            Currency = m.Currency,
            ProductCode = m.ProductCode,
            MetricCode = m.MetricCode
        };
    }

}


