
using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
    public  class  AddQuantityMetricCommand 
        : IRequest<ProductMetricDto>
    {
        public Guid ProductId { get; set; }

        public Guid MetricId { get; set; }

        public decimal Value { get; set; }

        public DateTime EffectiveDate { get; set; }        = DateTime.MinValue;

   
        public AddQuantityMetricCommand(Guid ProductId, Guid MetricId, decimal Value,
                                       DateTime EffectiveDate)
        {
            this.ProductId = ProductId;
            this.MetricId = MetricId;
            this.Value = Value;
            this.EffectiveDate = EffectiveDate; 
        }

 
    }
}
