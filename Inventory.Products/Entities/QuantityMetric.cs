using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Entities;

/// <summary>
/// Customizable product metrics using metric and productId and effective date as PK 
/// 
/// </summary>
public class QuantityMetric
{
    public QuantityMetric()
    {

    }
    public QuantityMetric(Guid ProductId, DateTime EffectiveDate, bool IsCancelled)
    {
        this.ProductId = ProductId;
        this.EffectiveDate = EffectiveDate; 
        this.IsCancelled = IsCancelled; 
    }

    public Guid TransactionId { get; set; }
    public decimal Diff { get; set; }
    public bool IsCancelled { get; set; }
    public Guid ProductId { get; set; }
    public decimal Value { get; set; }
    public DateTime EffectiveDate { get; set; } = DateTime.MinValue;
    public ModificationType ModificationType { get; set; }  


    public  static QuantityMetric CreateQuantityMetric(QuantityMetricDto m)
    {
        return new QuantityMetric()
        {
            EffectiveDate = m.EffectiveDate,
            ProductId = m.ProductId,
            Value = m.Value            
        };
    }
}


