using Inventory.Expressions.Contracts;
using Inventory.Expressions.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Expressions.Handlers
{
    public class GetBooleanExpressionsHandler :
    IRequestHandler<GetBooleanExpressionsQuery,
        List<BooleanExpressionDto>>
    {

        private readonly ExpressionsDbContext _context;

        public GetBooleanExpressionsHandler(ExpressionsDbContext context)
        {
            _context = context;
        }

        public async  Task<List<BooleanExpressionDto>> 
            Handle(GetBooleanExpressionsQuery request, CancellationToken cancellationToken)
        {


                 return   await   _context.BooleanExpressions
                .Select(o=> new BooleanExpressionDto(o.Id, o.Expression,
                                        o.RunEveryMinutes, o.InventoryId))
                .ToListAsync();

            
        }
    }
}
