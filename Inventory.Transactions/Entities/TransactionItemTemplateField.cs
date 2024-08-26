namespace Inventory.Transactions.Entities
{


    /// <summary>
    /// configuration item
    /// describes the field for an transaction template
    /// </summary>
    public  class TransactionItemTemplateField
    {
        public  Guid Id { get; set; }
        /// <summary>
        /// total price 
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        ///example  unitprice * quantity 
        /// </summary>
        public string Expression { get; set; } = string.Empty;

        public TemplateFieldType Type { get; set; }

        public ICollection<TransactionItemFieldValue> FieldValues { get; set; } = new List<TransactionItemFieldValue>();




        //todo: do i need this here? since it is visual ? some other 
        // entity?
        /// <summary>
        /// the order for which the item appears?
        /// 
        /// </summary>
        // public int VisualOrder { get; set; }
    }
}
