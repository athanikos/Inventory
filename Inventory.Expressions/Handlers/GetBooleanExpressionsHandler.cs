using Inventory.Expressions.Contracts;
using Inventory.Expressions.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Expressions.Handlers
{
    public class GetBooleanExpressionsHandler(ExpressionsDbContext context) :
            IRequestHandler<GetBooleanExpressionsQuery,
                List<BooleanExpressionDto>>
    {
        public async Task<List<BooleanExpressionDto>>
            Handle(GetBooleanExpressionsQuery request, CancellationToken cancellationToken) =>
            await context.BooleanExpressions
                .Select(o => new BooleanExpressionDto(o.Id, o.Expression,
                    o.RunEveryMinutes, o.InventoryId))
                .ToListAsync(cancellationToken: cancellationToken);
    }
}
