namespace Expressions.Entities
{
    // <summary>
    // TOTAL VALUE = SUM( VALUE (ADA,UpperBoundDate) )
    //  or
    //  VALUE([ADA, XRP] , UpperBoundDate) )
    //  or
    //  VALUE(ALL , UpperBound)
    // </summary>
    public  class InventoryExpression
    {
        public Guid Id { get; set; }

        public string Expression { get; set; } = string.Empty;

        public int RunEveryMinutes { get; set; }

        public Guid TargetInventoryId { get; set; }

        public Guid TargetMetricId { get; set; }  
    }
}
