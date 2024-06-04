using Inventory.Products.Repositories;
using MediatR;
using Transaction.Products.Endpoints;

namespace Inventory.Products.Handlers
{
    internal class DeleteTransactionHandler   :
        IRequestHandler<DeleteTransactionCommand>
    {
        private readonly ITransactionRepository _repo;

        public DeleteTransactionHandler(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle
            (DeleteTransactionCommand request, 
            CancellationToken cancellationToken)
        {
            await            _repo.DeleteTransactionAsync(new Dto.TransactionDto(request.Id, string.Empty, DateTime.MinValue));
        }

    
    }
}
