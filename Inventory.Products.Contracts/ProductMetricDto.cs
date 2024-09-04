namespace Inventory.Products.Contracts.Dto
{

    public class ProductMetricDto
    {
        public ProductMetricDto(Guid productId)
        {
            ProductId = productId;
        }



        public ProductMetricDto(
            Guid productId, 
            Guid metricId,
            decimal value, 
            DateTime effectiveDate, 
            string currency,
            string productCode,
            string metricCode
            )
        {
            ProductId = productId;
            MetricId = metricId;    
            Value = value;
            EffectiveDate = effectiveDate;
            Currency = currency;
            ProductCode = productCode;
            MetricCode = metricCode;    
        }

        public  static ProductMetricDto NewProductMetricDto(Guid metricId, Guid productId,
         int quantity, string currency, string productCode, string metricCode)
        {
            return new ProductMetricDto(productId, metricId, quantity, DateTime.MinValue, currency, productCode, metricCode);
        }


        public Guid ProductId { get; set; }

        public Guid MetricId { get; set; }

        public string ProductCode { get; set; } = string.Empty;

        public string MetricCode { get; set; } = string.Empty;

        public decimal Value { get;  set; }

        public DateTime EffectiveDate { get; set; }
        = DateTime.MinValue;

        public string Currency { get; set; } = string.Empty;

        public Guid SourceId { get; set; }


        public override string ToString()
        {
            return "ProductId" +
            ProductId.ToString() + " "+
           "MetricId" + MetricId.ToString() + " "+
            Value + " " +  EffectiveDate;
        }
    }

}