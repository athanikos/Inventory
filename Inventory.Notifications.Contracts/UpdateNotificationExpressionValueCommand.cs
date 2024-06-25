namespace Inventory.Notifications.Contracts
{

    public  class UpdateNotificationExpressionValueCommand
    {
        public Guid BooleanExpressionId { get; set; }

        public bool ExpressionValue { get; set; } 
    }
}
