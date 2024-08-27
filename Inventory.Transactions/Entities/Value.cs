namespace Inventory.Transactions.Entities
{
    /// <summary>
    /// represents the actual computed value for a transaction field 
    /// </summary>
    public  class Value
    {
        public Guid  Id { get; set; }
        /// <summary>
        /// depending on type in TransactionItemTemplateField 
        /// this can be any value of 
        /// date , datetime, decimal , integer etc 
        /// </summary>
        public string Text { get; set; } = string.Empty;

        public Guid FieldId { get; set; }

        public required Field Field { get; set; }

        public Transaction? Transaction { get; set; }

        public Entity? Entity { get; set; }

        /// <summary>
        /// either this or entityId is null 
        /// </summary>
        public Guid? TransactionId { get; set; }

        /// <summary>
        /// either this or TransactionId is null 
        /// </summary>
        public Guid? EntityId { get; set; }

    }
}
