namespace Inventory.Products.Contracts.Dto
{

    public class QuantityMetricDto
    {
        public QuantityMetricDto(
            Guid productId, 
            decimal value, 
            DateTime effectiveDate 
         
            )
        {
            ProductId = productId;
            Value = value;
            EffectiveDate = effectiveDate;
        }

        public static QuantityMetricDto NewQuantityMetricDto(
                                        Guid productId, decimal value,
                                        DateTime effectiveDate)
        {
            return new QuantityMetricDto(productId, value, effectiveDate);
        }

        public Guid ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public decimal Value { get;  set; }
        public DateTime EffectiveDate { get; set; }  = DateTime.MinValue;

        public override string ToString()
        {
            return nameof(ProductId) +  ProductId.ToString() + " " +
                   Value + " "  +  EffectiveDate;
        }
    }

}