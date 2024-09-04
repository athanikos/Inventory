﻿using System.ComponentModel.DataAnnotations;

namespace Inventory.Products.Entities;

/// <summary>
/// Customizable product metrics using metric and productId and effective date as PK 
/// 
/// </summary>
public class QuantityMetric
{
    public Guid ProductId { get; set; }
     
    public string ProductCode { get; set; } = string.Empty;
        
    public decimal  Value { get; set; }

    public DateTime EffectiveDate { get;  set; }
    = DateTime.MinValue;
}


