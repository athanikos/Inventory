using Inventory.Products.Contracts;

namespace Inventory.Products.Entities
{

    /// <summary>
    /// Quantity is treated as an isolated product metric to allow table locks 
    /// for increment / decrement operations
    /// </summary>
    public class ProductQuantityMetric
   {
        public Guid ProductId { get; set; }

        public Guid MetricId { get; set; }

        public string ProductCode { get; set; } = string.Empty;

        public  string MetricCode { get; set; } = Constants.Quantitycode;

        public decimal Value { get; internal set; }

        // represents to the value added / subtracted when (buy/selling/let/unlet )
        //todo do  ineed this ?
        public decimal AddValue {  get; set; } 

        public DateTime EffectiveDate { get; internal set; }= DateTime.MinValue;
    }
}
