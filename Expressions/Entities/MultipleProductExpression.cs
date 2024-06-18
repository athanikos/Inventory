namespace Inventory.Expressions.Entities
{
    // <summary>
    // TOTAL VALUE = SUM( VALUE (ADA,UpperBoundDate) )
    //  or
    //  VALUE([ADA, XRP] , UpperBoundDate) )
    //  or
    //  VALUE(ALL , UpperBound)
    // </summary>
    public  class MultipleProductExpression
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public int RunEveryMinutes { get; set; }

        public Guid InventoryId { get; set; }
    }
}
