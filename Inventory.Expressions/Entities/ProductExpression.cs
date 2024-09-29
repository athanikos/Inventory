namespace Inventory.Expressions.Entities
{
    /// <summary>
    /// QUANTITY(ADA,LATEST)
    /// QUANTITY(ADA,LATEST) * PRICE(ADA,LATEST)
    /// </summary>
    public class ProductExpression
    {
        public Guid Id { get; set; }

        public string Expression { get; set; } = string.Empty;

        public Guid InventoryId { get; set; }

        public int RunEveryMinutes { get; set; }

        public Guid TargetProductId { get; set; } 

        public Guid TargetMetricId { get; set; }
    }

}
