
namespace TransactionItem.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;
    using Azure.Core;
    using Inventory.Products.Repositories;

    public class AddTransactionItem :
        Endpoint<AddTransactionItemRequest>
    {
        private readonly ITransactionRepository _repo;

        public  AddTransactionItem(ITransactionRepository repo)
        {
            _repo = repo;
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

            TransactionItemDto trns =
               new TransactionItemDto(
                 req.TransactionId,
                 req.Id,
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
                 req.FinalPrice
                 );
            await _repo.EditTransactionItemAsync(trns);

            return TypedResults.Ok(trns);
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
