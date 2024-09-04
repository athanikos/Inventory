using Expressions.Entities;
using Inventory.Products;

namespace Inventory.Expressions.Repositories
{
    public  class PostgresRepository : IExpressionRepository
    {

        public PostgresRepository(ExpressionsDbContext context)
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
