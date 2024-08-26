namespace Inventory.Transactions.Entities
{
    /// <summary>
    /// configuration holder for transacationItems depedning on 
    /// the type of transaction 
    /// examples can be Let, Retail , Cryptos 
    /// </summary>
    public  class TransactionItemTemplate
    {
        public Guid  Id { get; set; }

        public string Name { get; set; }    = string.Empty;

        public List<TransactionItemTemplateField> TemplateFields { get; set; } = new();
    }


}
