
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
        Endpoint<DeleteTransactionRequest>
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

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteTransactionRequest req,
                        CancellationToken ct)
        {
            var command = new DeleteTransactionCommand(
                req.Id);
            
            await _mediator!.
                Send(command, ct);

            return TypedResults.Ok();
        }
    }


    public record DeleteTransactionRequest(Guid Id);

    public record DeleteTransactionCommand(Guid Id)
      : IRequest;

  
}
