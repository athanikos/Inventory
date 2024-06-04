using MediatR;
using Inventory.Products.Dto;
using Transaction.Products.Endpoints;
using Inventory.Products.Endpoints;
using Inventory.Products.Repositories;

namespace Inventory.Products.Handlers
{
    public class EditTransactionHandler   :
        IRequestHandler<EditTransactionCommand, TransactionDto>
    {
        private readonly ITransactionRepository _repo;

        public EditTransactionHandler(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public async Task<TransactionDto> Handle
            (EditTransactionCommand request, 
            CancellationToken cancellationToken)
        {
            return await   _repo.AddTransactionAsync(new TransactionDto(request.Id, request.Description, request.Created));
        }
            
    }
}
