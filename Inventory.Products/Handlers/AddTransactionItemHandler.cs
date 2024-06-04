using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using TransactionItem.Products.Endpoints;
using Inventory.Products.Entities;
using Inventory.Products.Repositories;

namespace Inventory.Products.Handlers
{
    internal class AddTransactionItemHandler :
        IRequestHandler<AddTransactionItemCommand, TransactionItemDto>
    {
        private readonly ITransactionRepository _repository;

        public AddTransactionItemHandler(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<TransactionItemDto> Handle
            (AddTransactionItemCommand request, 
            CancellationToken cancellationToken)
        {

            TransactionItemDto trns =
               new TransactionItemDto(
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
                 );
           return  await _repository.EditTransactionItemAsync(trns);

        }

    
    }
}
