
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;

    public class EditTransactionItem :
        Endpoint<EditTransactionItemRequest>
    {
        private readonly IMediator _mediator;

        public  EditTransactionItem(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/transactionitem");
            // to do claims this is per TransactionItemId claim
            //  something like Admin_<TransactionItemId>
        }

        public override async Task<Results<Ok<TransactionItemDto>, NotFound, ProblemDetails>>
            HandleAsync(EditTransactionItemRequest req,
                        CancellationToken ct)
        {
            var command = new EditTransactionItemCommand(
                       req.Id,
                       req.TransactionId,
                       req.Description,
                       req.TransactionType,
                       req.UnitPrice,
                       req.Quantity,
                       req.Price,
                       req.VatPercentage,
                       req.PriceAfterVat,
                       req.Discount,
                       req.DiscountAmount,
                       req.TransactionFees,
                       req.DeliveryFees,
                       req.FinalPrice);

            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<TransactionItemDto>(result);
        }
    }


    public record EditTransactionItemRequest(Guid TransactionId,
                    Guid Id,
                    string Description,
                    string TransactionType,
                    decimal UnitPrice,
                    decimal Quantity,
                    decimal Price,
                    decimal VatPercentage,
                    decimal PriceAfterVat,
                    decimal Discount,
                    decimal DiscountAmount,
                    decimal TransactionFees,
                    decimal DeliveryFees,
                     decimal FinalPrice);

    public record EditTransactionItemCommand(Guid TransactionId,
                    Guid Id,
                    string Description,
                    string TransactionType,
                    decimal UnitPrice,
                    decimal Quantity,
                    decimal Price,
                    decimal VatPercentage,
                    decimal PriceAfterVat,
                    decimal Discount,
                    decimal DiscountAmount,
                    decimal TransactionFees,
                    decimal DeliveryFees,
                    decimal FinalPrice)
      : IRequest<TransactionItemDto>;

  
}
