namespace Inventory.Expressions.Entities
{

    /// <summary>
    ///  many to many table for aggreagtes expressions 
    ///  keeps track of products being part of an expression 
    ///  computed by system when expression in aggregate expression is evaluated 
    /// </summary>
    public  class AggregateExpressionProduct
    {
        public Guid ProductId { get; set; } 
        public Guid MultipleExpressionId { get; set; } 
    }
}
