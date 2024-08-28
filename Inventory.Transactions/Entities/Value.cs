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

        public Guid? TransactionId { get; set; }

        /// <summary>
        /// the line in which this value appears in section 
        /// </summary>
        public int SectionLineNumber { get; set; } = 0;

    }
}
