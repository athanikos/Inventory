using Inventory.Transactions.Contracts;

namespace Inventory.Transactions.Entities
{
    public  class Section
    {
        public Guid Id { get; set; }

        public Guid    TemplateId { get; set; }

        public string Name { get; set; } = string.Empty;       

        public List<Field> Fields { get; set; } = new();

        public SectionType SectionType { get; set; }    
        
    }
}
