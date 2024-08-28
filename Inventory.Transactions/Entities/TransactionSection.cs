using Inventory.Transactions.Contracts;

namespace Inventory.Transactions.Entities
{
    public class TransactionSection
    {
        Guid Id { get; set; }
        public Guid TransactionId { get; set; } = Guid.Empty;
        public SectionType TransactionSectionType {get; set;}

        public required Transaction Transaction { get; set; }

        public ICollection<TransactionSectionGroup> SectionGroups { get; set; } 
        = new List<TransactionSectionGroup>();

    }
}
