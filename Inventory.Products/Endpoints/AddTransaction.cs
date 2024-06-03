
namespace Transaction.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;

    public class AddTransaction :
        Endpoint<AddTransactionRequest>
    {
        private readonly IMediator _mediator;

        public  AddTransaction(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("/Transaction");
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TransactionDto>, NotFound, ProblemDetails>>
            HandleAsync(AddTransactionRequest req,
                        CancellationToken ct)
        {
            var command = new AddTransactionCommand(
                req.Description);
            var result = await _mediator!.
                Send(command, ct);

            return TypedResults.Ok<TransactionDto>(result);
        }
    }


    public record AddTransactionRequest(string Description);

    public record AddTransactionCommand(string Description)
      : IRequest<TransactionDto>;

  
}
