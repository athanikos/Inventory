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
    /// <summary>
    ///  the value used to add/ subtarct from previous record value that computes Value 
    /// </summary>
    public decimal Diff { get; set; }
    public bool IsCancelled { get; set; }   
    public Guid ProductId { get; set; }
    /// <summary>
    /// contains previous value + Diff 
    /// </summary>
    public decimal  Value { get; set; } 
    public DateTime EffectiveDate { get;  set; }  = DateTime.MinValue;

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


