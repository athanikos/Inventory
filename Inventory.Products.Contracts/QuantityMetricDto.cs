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
            MetricCode = Constants.QUANTITYCODE;
        }

        public Guid ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string MetricCode { get; } = Constants.QUANTITYCODE;
        public decimal Value { get;  set; }
        public DateTime EffectiveDate { get; set; }  = DateTime.MinValue;
        

        public override string ToString()
        {
            return      
            "ProductId" +
            ProductId.ToString() + " "+
            Value + " " +
            EffectiveDate;
        }
    }

}