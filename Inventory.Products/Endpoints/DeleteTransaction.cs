
namespace Transaction.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;

    public class EditTransaction :
        Endpoint<EditTransactionRequest>
    {
        private readonly IMediator _mediator;

        public  EditTransaction(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Delete("/transaction");
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TransactionDto>, NotFound, ProblemDetails>>
            HandleAsync(EditTransactionRequest req,
                        CancellationToken ct)
        {
            var command = new EditTransactionCommand(
                req.Description);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<TransactionDto>(result);
        }
    }


    public record EditTransactionRequest(string Description);

    public record EditTransactionCommand(string Description)
      : IRequest<TransactionDto>;

  
}
