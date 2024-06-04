using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using TransactionItem.Products.Endpoints;
using Inventory.Products.Repositories;

namespace Inventory.Products.Handlers
{
    internal class EditTransactionItemHandler :
    
        IRequestHandler<EditTransactionItemCommand,
        TransactionItemDto>
    {
        private readonly ITransactionRepository _repo;

        public EditTransactionItemHandler(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public async Task<TransactionItemDto> Handle
            (EditTransactionItemCommand request, 
            CancellationToken cancellationToken)
        {

            return await _repo.EditTransactionItemAsync(new TransactionItemDto(
                 request.TransactionId,
                 request.Id,
                 request.Description,
                 request.TransactionType,
                 request.UnitPrice,
                 request.Quantity,
                 request.Price,
                 request.VatPercentage,
                 request.PriceAfterVat,
                 request.Discount,
                 request.DiscountAmount,
                 request.TransactionFees,
                 request.DeliveryFees,
                 request.FinalPrice
                 ));
       
        }

    
    }
}
