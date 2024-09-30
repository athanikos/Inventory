using Inventory.Expressions.Dto;
using MediatR;


namespace Inventory.Expressions.Contracts
{
    public record GetBooleanExpressionsQuery
         : IRequest<List<BooleanExpressionDto>>;
}
