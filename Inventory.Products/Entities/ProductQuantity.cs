
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Products.Entities
{

    /// <summary>
    /// Quantity is treated as an isolated product metric to allow table locks 
    /// for increment / decrement operations
    /// </summary>
    public class ProductQuantity
    {
        public Guid ProductId { get; set; }

        public Guid MetricId { get; set; }

        public string ProductCode { get; set; } = string.Empty;

        public  string MetricCode { get; set; } = "QUANTITY";

        public decimal Value { get; internal set; }

        public DateTime EffectiveDate { get; internal set; }= DateTime.MinValue;
    }
}
