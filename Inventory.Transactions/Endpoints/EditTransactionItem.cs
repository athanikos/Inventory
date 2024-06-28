
namespace Inventory.Transactions.Endpoints
{
    using System.Threading;
    using System.Threading.Tasks;
    using FastEndpoints;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;
    using Microsoft.AspNetCore.Http.HttpResults;

    public class EditTransactionItem :
        Endpoint<EditTransactionItemRequest>
    {
        private readonly ITransactionRepository _repo;

        public  EditTransactionItem(ITransactionRepository repo)
        {
            _repo = repo;
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
        
            throw new NotImplementedException();

            //var dto =  await _repo.EditTransactionItemAsync(new TransactionItemDto(
            //   req.TransactionId,
            //req.Id,
            //   req.Description,
            //   req.TransactionType,
            //   req.UnitPrice,
            //req.Quantity,
            //req.Price,
            //   req.VatPercentage,
            //   req.PriceAfterVat,
            //   req.Discount,
            //   req.DiscountAmount,
            //   req.TransactionFees,
            //   req.DeliveryFees,
            //   req.FinalPrice
            //   ));

            //return TypedResults.Ok<TransactionItemDto>(dto);
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

   

  
}
