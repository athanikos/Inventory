using Inventory.Expressions.Entities;

namespace Inventory.Expressions.Repositories
{
    public  interface IExpressionRepository
    {
        List<ProductExpression> GetProductExpressions();

        List<InventoryExpression> GetInventoryExpressions();

        List<BooleanExpression> GetBooleanExpressions();
    }
}
