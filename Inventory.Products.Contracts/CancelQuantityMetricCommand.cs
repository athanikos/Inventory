
using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
    public class CancelQuantityMetricCommand(Guid transactionId) : IRequest<List<QuantityMetricDto>>
    {
         public Guid TransactionId { get; } = transactionId;
    }
}
