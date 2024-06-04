using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Transaction.Products.Endpoints;
using Inventory.Products.Repositories;

namespace Inventory.Products.Handlers
{
    internal class AddTransactionHandler :
        IRequestHandler<AddTransactionCommand, TransactionDto>
    {
        private readonly ITransactionRepository _repo;

        public AddTransactionHandler(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public async Task<TransactionDto> Handle
            (AddTransactionCommand request, 
            CancellationToken cancellationToken)
        {
           return await  _repo.AddTransactionAsync(new TransactionDto(request.TransactionId, request.Description, request.Created));
        }

    
    }
}
