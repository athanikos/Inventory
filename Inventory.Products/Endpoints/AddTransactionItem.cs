
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
            Post("/TransactionItem");
            // to do claims this is per TransactionItemId claim
            //  something like Admin_<TransactionItemId>
        }

        public override async Task<Results<Ok<TransactionItemDto>, NotFound, ProblemDetails>>
            HandleAsync(AddTransactionItemRequest req,
                        CancellationToken ct)
        {
            var command = new AddTransactionItemCommand(
                req.Description);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<TransactionItemDto>(result);
        }
    }


    public record AddTransactionItemRequest(string Description);

    public record AddTransactionItemCommand(string Description)
      : IRequest<TransactionItemDto>;

  
}
