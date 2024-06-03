
namespace TransactionItem.Products.Endpoints
{
    using FastEndpoints;
     using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;
    using Inventory.Products.Entities;

    public class DeleteTransactionItem :
        Endpoint<DeleteTransactionItemRequest>
    {
        private readonly IMediator _mediator;

        public DeleteTransactionItem(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Delete("/transactionitem");
            // to do claims this is per TransactionItemId claim
            //  something like Admin_<TransactionItemId>
        }

        public override async Task<Results<Ok<TransactionItemDto>, NotFound, ProblemDetails>>
            HandleAsync(DeleteTransactionItemRequest req,
                        CancellationToken ct)
        {
            var command = new DeleteTransactionItemCommand(
                       req.Id
                   );

            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<TransactionItemDto>(result);
        }
    }


    public record DeleteTransactionItemRequest(
                    Guid Id);

    public record DeleteTransactionItemCommand(Guid Id
               )
      : IRequest<TransactionItemDto>;

  
}
