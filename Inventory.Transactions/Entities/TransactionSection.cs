using Inventory.Transactions.Contracts;

namespace Inventory.Transactions.Entities
{
    public class TransactionSection
    {
        public  Guid Id { get; set; }
        public Guid TransactionId { get; set; } = Guid.Empty;

        public string Name { get; set; }    = string.Empty; 

        public SectionType TransactionSectionType { get; set;}

        public Transaction Transaction { get; set; } = null;

        public ICollection<TransactionSectionGroup> SectionGroups { get; set; } 
        = [];

    }
}
