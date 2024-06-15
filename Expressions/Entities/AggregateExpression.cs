namespace Inventory.Expressions.Entities
{


    /// <summary>
    /// SUM(
    /// </summary>
    public  class AggregateExpression
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public Guid MetricId { get; set; }

        public Guid ProductId { get; set; }

        public int RunEveryMinutes { get; set; }
    }
}
