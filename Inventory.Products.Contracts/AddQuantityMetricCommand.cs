
using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
    public  class  AddQuantityMetricCommand 
        : IRequest<QuantityMetricDto>
    {
        public Guid ProductId { get; set; }

        public decimal Value { get; set; }

        public DateTime EffectiveDate { get; set; }        = DateTime.MinValue;
                
        public AddQuantityMetricCommand(Guid ProductId, decimal Value,
                                       DateTime EffectiveDate)
        {
            this.ProductId = ProductId;
            this.Value = Value;
            this.EffectiveDate = EffectiveDate; 
        }

 
    }
}
