using Inventory.Products.Endpoints;
using Inventory.Products.Entities;
using MediatR;
using Inventory.Products.Dto;

namespace Inventory.Products.Handlers
{
    internal class EditInventoryHandler :
        IRequestHandler<AddInventoryCommand, InventoryDto>
    {
        private readonly ProductsDbContext _context;

        public EditInventoryHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<InventoryDto> Handle
            (AddInventoryCommand request, 
            CancellationToken cancellationToken)
        {
            Inventory.Products.Entities.Inventory inv = 
                new Inventory.Products.Entities.Inventory()
            { Description = request.Description };
            _context.Inventories.Add(inv);
            _context.Entry(inv).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return new InventoryDto(inv.Id, inv.Description);

        }

    
    }
}
