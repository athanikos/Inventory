
using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
    public abstract class AddQuantityMetricCommand(
        Guid productId,
        decimal value,
        DateTime effectiveDate,
        Guid transactionId,
        decimal diff,
        bool isCancelled)
        : IRequest<QuantityMetricDto>
    {
        public Guid ProductId { get; set; } = productId;
        public decimal Value { get; set; } = value;
        public DateTime EffectiveDate { get; set; } = effectiveDate;
        public Guid TransactionId { get; set; } = transactionId;
        public decimal Diff { get; set; } = diff;
        public bool IsCancelled { get; set; } = isCancelled;
    }
}
