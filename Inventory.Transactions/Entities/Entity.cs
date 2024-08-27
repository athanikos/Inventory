namespace Inventory.Transactions.Entities
{
    /// <summary>
    /// represents a person or a company  
    /// </summary>
    public  class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public ICollection<Value> Values { get; set; } = new List<Value>();

        public Contracts.EntityType EntityType { get; set; }  
        
    }
}
