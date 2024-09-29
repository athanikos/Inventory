using Expressions.Entities;
using Inventory.Products;

namespace Inventory.Expressions.Repositories
{
    public  class PostgresExpressionRepository : IExpressionRepository
    {

        public PostgresExpressionRepository(ExpressionsDbContext context)
        { _context = context; }

        private readonly ExpressionsDbContext _context;


        public List<ProductExpression> GetProductExpressions()
        {
            return [.. _context.ProductExpressions];
        }

        public List<InventoryExpression> GetInventoryExpressions()
        {
            return [.. _context.InventoryExpressions];
        }

        public List<BooleanExpression> GetBooleanExpressions()
        {
            return [.. _context.BooleanExpressions];
        }
    }
}
