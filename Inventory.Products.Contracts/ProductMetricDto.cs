using MediatR;

namespace Inventory.Products.Contracts.Dto
{

    public class ProductMetricDto
    {
        public ProductMetricDto() {  }
       
        public ProductMetricDto(Guid productId) { ProductId = productId; }

        public ProductMetricDto(
            Guid productId, 
            Guid metricId,
            decimal value, 
            DateTime effectiveDate, 
            string productCode,
            string metricCode,
            Guid     unitOfMeasurementId
            )
        {
            ProductId = productId;
            MetricId = metricId;    
            Value = value;
            EffectiveDate = effectiveDate; 
            ProductCode = productCode;
            MetricCode = metricCode;
            UnitOfMeasurementId = unitOfMeasurementId;
        }

        public  static ProductMetricDto NewProductMetricDto(Guid metricId, Guid productId,
            int quantity, string productCode, string metricCode, Guid unitOfMeasurementId)
        {
            return new ProductMetricDto(productId, metricId, quantity, 
                DateTime.MinValue,
                 productCode, metricCode, unitOfMeasurementId);
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

        public Guid UnitOfMeasurementId { get; set; }
        
        public override string ToString()
        {
            return "ProductId" +
            ProductId.ToString() + " "+
           "MetricId" + MetricId.ToString() + " "+
            Value + " " +  EffectiveDate;
        }
    }

}