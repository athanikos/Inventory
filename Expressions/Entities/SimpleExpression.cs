namespace Expressions.Entities
{
    /// <summary>
    /// QUANTITY(ADA,LATEST)
    /// QUANTITY(ADA,LATEST) * PRICE(ADA,LATEST)
    /// </summary>
    public class SimpleExpression
    {
        public Guid Id { get; set; }

        public string Expression { get; set; } = string.Empty;

        public Guid SourceMetricId { get; set; }  
        
        public Guid ProductId  { get; set; }

        public int RunEveryMinutes { get; set; }

    }

}
