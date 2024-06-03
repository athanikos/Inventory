﻿using MediatR;
using Inventory.Products.Dto;
using Transaction.Products.Endpoints;

namespace Inventory.Products.Handlers
{
    internal class EditTransactionHandler   :
        IRequestHandler<AddTransactionCommand, TransactionDto>
    {
        private readonly ProductsDbContext _context;

        public EditTransactionHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionDto> Handle
            (AddTransactionCommand request, 
            CancellationToken cancellationToken)
        {
            Entities.Transaction trns =  new Entities.Transaction()    
            { Description = request.Description };

            _context.Transactions.Add(trns);
            await _context.SaveChangesAsync(cancellationToken);

            return new TransactionDto(trns.Id, trns.Description,DateTime.Now);

        }

    
    }
}
