namespace Expressions.Entities
{

    /// <summary>
    /// PRICE(ADA) > 0.5 
    /// </summary>
    public class BooleanExpression
    {
        
        public Guid Id { get; set; }

        public string Expression { get; set; } = string.Empty; //RICE(ADA) > 0.5 

        public Guid InventoryId { get; set; }

        public int RunEveryMinutes { get; set; }
   
    }
}
