namespace Inventory.Products.Contracts.Dto
{

    public class QuantityMetricDto
    {
        public QuantityMetricDto(
            Guid productId, 
            decimal value, 
            DateTime effectiveDate, 
            string productCode
            )
        {
            ProductId = productId;
            Value = value;
            EffectiveDate = effectiveDate;
            ProductCode = productCode;
        }

        public static QuantityMetricDto NewQuantityMetricDto(
                                        Guid productId, decimal value,
                                        string productCode, DateTime effectiveDate)
        {
            return new QuantityMetricDto(productId, value, effectiveDate, productCode);
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