
using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
    public  class  CancellQuantityMetricCommand 
        : IRequest<List<QuantityMetricDto>>
    {
         public Guid TransactionId { get; set; }
      
        public CancellQuantityMetricCommand(Guid TransactionId)
        {
            this.TransactionId = TransactionId; 
        }

 
    }
}
