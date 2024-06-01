using Product.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Entities = Inventory.Products.Entities;

namespace Product.Products.Handlers
{
    internal class EditProductHandler :
        IRequestHandler<AddProductCommand, ProductDto>
    {
        private readonly Inventory.Products.ProductsDbContext _context;

        public EditProductHandler(Inventory.Products.ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> Handle
            (AddProductCommand request, 
            CancellationToken cancellationToken)
        {
            Entities.Product prd =
                new Entities.Product()
            { Description = request.Description };
            _context.Products.Add(prd);
            _context.Entry(prd).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return new ProductDto(prd.Id, prd.Description);

        }

    
    }
}
