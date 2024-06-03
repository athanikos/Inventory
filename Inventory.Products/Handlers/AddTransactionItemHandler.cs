using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using TransactionItem.Products.Endpoints;
using Inventory.Products.Entities;

namespace Inventory.Products.Handlers
{
    internal class AddTransactionItemHandler :
        IRequestHandler<AddTransactionItemCommand, TransactionItemDto>
    {
        private readonly ProductsDbContext _context;

        public AddTransactionItemHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionItemDto> Handle
            (AddTransactionItemCommand request, 
            CancellationToken cancellationToken)
        {
            Entities.TransactionItem trns =  new Entities.TransactionItem()    
            { Description = request.Description };

            _context.TransactionItems.Add(trns);
            await _context.SaveChangesAsync(cancellationToken);

            return new TransactionItemDto(
                       trns.TransactionId,
                       trns.Id,
                       trns.Description,
                       trns.TransactionType,
                       trns.UnitPrice,
                       trns.Quantity,
                       trns.Price,
                       trns.VatPercentage,
                       trns.PriceAfterVat,
                       trns.Discount,
                       trns.DiscountAmount,
                       trns.TransactionFees,
                       trns.DeliveryFees,
                       trns.FinalPrice
                       );

        }

    
    }
}
