using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using TransactionItem.Products.Endpoints;

namespace Inventory.Products.Handlers
{
    internal class EditTransactionItemHandler :
    
        IRequestHandler<EditTransactionItemCommand,
        TransactionItemDto>
    {
        private readonly ProductsDbContext _context;

        public EditTransactionItemHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionItemDto> Handle
            (EditTransactionItemCommand request, 
            CancellationToken cancellationToken)
        {
            Entities.TransactionItem trns =
            new Entities.TransactionItem()
            {
                Description = request.Description
                ,
                Id = request.Id
                ,
                TransactionId = request.TransactionId
                ,
                TransactionType = request.TransactionType
                ,
                UnitPrice = request.UnitPrice
                ,
                Quantity = request.Quantity
                ,
                Price = request.Price
                ,
                VatPercentage = request.VatPercentage
                ,
                PriceAfterVat = request.PriceAfterVat
                ,
                Discount = request.Discount
                ,
                DiscountAmount = request.DiscountAmount
                ,
                TransactionFees = request.TransactionFees
                ,
                DeliveryFees = request.DeliveryFees
                ,
                FinalPrice = request.FinalPrice
            };


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
