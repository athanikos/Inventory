using Inventory.Products.Repositories;
using MediatR;
using TransactionItem.Products.Endpoints;

namespace Inventory.Products.Handlers
{
    internal class DeleteTransactionItemHandler :
        IRequestHandler<DeleteTransactionItemCommand>
    {
        private readonly ITransactionRepository _repo;

        public DeleteTransactionItemHandler(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle
            (DeleteTransactionItemCommand request, 
            CancellationToken cancellationToken)
        {
            await _repo.DeleteTransactionItemAsync(new Dto.TransactionItemDto(request.Id,Guid.Empty));
        }

    
    }
}
