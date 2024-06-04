
namespace TransactionItem.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;

    public class AddTransactionItem :
        Endpoint<AddTransactionItemRequest>
    {
        private readonly IMediator _mediator;

        public  AddTransactionItem(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/transactionItem");
            // to do claims this is per TransactionItemId claim
            //  something like Admin_<TransactionItemId>
        }

        public override async Task<Results<Ok<TransactionItemDto>, NotFound, ProblemDetails>>
            HandleAsync(AddTransactionItemRequest req,
                        CancellationToken ct)
        {
            var command = new AddTransactionItemCommand(
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


    public record AddTransactionItemRequest(Guid TransactionId,
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

    public record AddTransactionItemCommand(Guid TransactionId,
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
