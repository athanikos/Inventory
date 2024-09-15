namespace Inventory.Transactions.Entities
{

    public sealed class Transaction 
    {
            public Guid Id { get; set; }  = Guid.NewGuid();
            public string Description { get; set; }             = string.Empty;
            public DateTime Created { get; set; }
        public ICollection<TransactionSection> TransactionSections { get; set; } = [];

            public Template Template { get; set; } = null;

            public Guid TemplateId { get; set; }        
    }
}

