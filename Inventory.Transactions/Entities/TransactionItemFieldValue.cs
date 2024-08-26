namespace Inventory.Transactions.Entities
{
    /// <summary>
    /// represents the actual computed value for a transaction field 
    /// </summary>
    public  class TransactionItemFieldValue
    {
        public Guid  Id { get; set; }
        /// <summary>
        /// depending on type in TransactionItemTemplateField 
        /// this can be any value of 
        /// date , datetime, decimal , integer etc 
        /// </summary>
        public string Value { get; set; } = string.Empty;

        public Guid TransactionItemTemplateFieldId { get; set; }

        // todo do i need this?
        public required TransactionItemTemplateField Field { get; set; }



    }
}
