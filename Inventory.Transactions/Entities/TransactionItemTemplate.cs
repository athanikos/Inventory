namespace Inventory.Transactions.Entities
{
    /// <summary>
    /// configuration holder 
    /// </summary>
    public  class TransactionItemTemplate
    {
        public Guid  Id { get; set; }

        public string Name { get; set; }    = string.Empty;

        public List<TransactionItemTemplateField> Fields { get; set; } = new();
    }
}
