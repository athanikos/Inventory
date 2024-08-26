namespace Inventory.Transactions.Entities
{
    /// <summary>
    /// configuration holder for transacationItems depedning on 
    /// the type of transaction 
    /// examples can be Let, Retail , Cryptos 
    /// </summary>
    public  class Template
    {
        public Guid  Id { get; set; }

        public string Name { get; set; }    = string.Empty;

        public List<Field> Fields { get; set; } = new();

        public TemplateType Type { get; set; }

        public DateTime Created { get;set; }
    }

}
