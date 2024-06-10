namespace Inventory.Products.Contracts.Dto
{

    public class ProductMetricDto
    {
        public Guid ProductId { get; set; }

        public Guid MetricId { get; set; }

        public decimal Value { get;  set; }

        public DateTime EffectiveDate { get; set; }
        = DateTime.MinValue;
    }

}