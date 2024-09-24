namespace Inventory.Transactions.Entities
{
    /// <summary>
    /// represents the actual computed value for a transaction field 
    /// </summary>
    public  class Value
    {
        public Guid  Id { get; set; } = Guid.Empty;
        /// <summary>
        /// depending on type in TransactionItemTemplateField 
        /// this can be any value of 
        /// date , datetime, decimal , integer etc 
        /// </summary>
        public string Text { get; set; } = string.Empty;

        public Guid FieldId { get; set; }

      //  public Field Field { get; set; } = null; 
        public Guid TransactionId { get; set; }

        public TransactionSectionGroup TransactionSectionGroup { get; set; } = null;

        public Guid   TransactionSectionGroupId {  get; set; }       


    }
}
