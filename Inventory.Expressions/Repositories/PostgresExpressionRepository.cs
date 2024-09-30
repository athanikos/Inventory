using Inventory.Expressions.Entities;

namespace Inventory.Expressions.Repositories
{
    public class PostgresExpressionRepository(ExpressionsDbContext context) : IExpressionRepository
    {
        public List<ProductExpression> GetProductExpressions()
        {
            return [.. context.ProductExpressions];
        }

        public List<InventoryExpression> GetInventoryExpressions()
        {
            return [.. context.InventoryExpressions];
        }

        public List<BooleanExpression> GetBooleanExpressions()
        {
            return [.. context.BooleanExpressions];
        }
    }
}
