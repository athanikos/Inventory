namespace Inventory.Products.Contracts.Dto
{

    public class QuantityMetricDto
    {
        public QuantityMetricDto(
            Guid productId, 
            Guid metricId,
            decimal value, 
            DateTime effectiveDate, 
             string productCode,
            string metricCode
            )
        {
            ProductId = productId;
            MetricId = metricId;    
            Value = value;
            EffectiveDate = effectiveDate;
            ProductCode = productCode;
            MetricCode = metricCode;    
        }

        public Guid ProductId { get; set; }

        public Guid MetricId { get; set; }

        public string ProductCode { get; set; } = string.Empty;

        public string MetricCode { get; set; } = string.Empty;

        public decimal Value { get;  set; }

        public DateTime EffectiveDate { get; set; }
        = DateTime.MinValue;

        public Guid SourceId { get; set; }


        public override string ToString()
        {
            return 
            
              "ProductId" +
            ProductId.ToString() + " "+
              "MetricId" + MetricId.ToString() + " "+
            Value + " " +
            EffectiveDate;
        }
    }

}