using Inventory.Products.Contracts.Dto;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Products.Entities;

/// <summary>
/// Customizable product metrics using metric and productId and effective date as PK 
/// 
/// </summary>
public class QuantityMetric
{
    public Guid ProductId { get; set; }
     
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


