
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

        public Guid TransactionId { get; set; }
        public decimal Diff { get; set; }
        public bool IsCancelled { get; set; }


        public AddQuantityMetricCommand(Guid ProductId, decimal Value,
                                       DateTime EffectiveDate, Guid TransactionId, decimal Diff, bool IsCancelled)
        {
            this.ProductId = ProductId;
            this.Value = Value;
            this.EffectiveDate = EffectiveDate; 
            this.TransactionId = TransactionId; 
            this.Diff = Diff;   
            this.IsCancelled = IsCancelled;     

        }

 
    }
}
