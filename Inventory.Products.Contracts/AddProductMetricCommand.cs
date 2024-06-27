
using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
    public  class
        AddProductMetricCommand : IRequest<ProductMetricDto>
    {
        public Guid ProductId { get; set; }

        public Guid MetricId { get; set; }

        public decimal Value { get; set; }

        public DateTime EffectiveDate { get; set; }        = DateTime.MinValue;

        public string Currency { get; set; } = string.Empty;


        public AddProductMetricCommand(Guid ProductId, Guid MetricId, decimal Value, DateTime EffectiveDate, string Curtrency)
        {
            this.ProductId = ProductId;
            this.MetricId = MetricId;
            this.Value = Value;
            this.EffectiveDate = EffectiveDate; 
            this.Currency = Curtrency;
        }

 
    }
}
