namespace Inventory.Notifications.Entities
{
    /// <summary>
    ///
    /// </summary>
    public  class Notification
    {
        public Guid Id { get; set; }
      
        public Guid BooleanExpressionId { get; set; }

        /// <summary>
        ///  expression is evaluated to true 
        /// </summary>
        public bool ExpressionValue { get; set; } = false;

        /// <summary>
        /// The job that asks
        /// </summary>
        public int NotifyEveryMinutes  { get; set; }

        /// <summary>
        /// -1 forever (as long as IsActive )
        /// </summary>
        public int NotifyTimes { get; set; }

        /// <summary>
        /// </summary>
        public bool IsActive { get; set; } = true;

        public DateTime SystemDate  { get; set; }
     
    }

}
