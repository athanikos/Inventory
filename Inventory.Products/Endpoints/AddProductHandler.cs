using FastEndpoints;
using Inventory.Products.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace Inventory.Products.Endpoints
{
    internal class AddProductHandler : IRequestHandler<AddProductCommand, ProductDto>
    {
        private readonly ProductsDbContext _context;

        public AddProductHandler(ProductsDbContext context)
        {
            _context = context;
        }

        public async  Task<ProductDto> Handle(AddProductCommand request,
            CancellationToken cancellationToken)
        {
            Product prd = new Product()
            {  Description = request.Description };

            _context.Products.Add(prd);
            await _context.SaveChangesAsync(cancellationToken);
          
            return new ProductDto()
            { Id = prd.Id, Description = prd.Description };
        }
    }
}
