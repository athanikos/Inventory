using Inventory.Transactions.Entities;

namespace Inventory.Transactions.Dto
{
    public  class TransactionSectionGroupDto
    {
        public Guid Id { get; set; } = Guid.Empty;

        public Guid TransactionSectionId { get; set; }
        
        public int GroupValue { get; set; }
        
        public ICollection<ValueDto> Values { get; set; } = new List<ValueDto>();

        public TransactionSectionDto TransactionSection { get; set; } = null;

    }
}
