using Inventory.Transactions.Contracts;
using Inventory.Transactions.Entities;
using System.Text.Json.Serialization;

namespace Inventory.Transactions.Dto
{
    public  class TransactionSectionDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid TransactionId { get; set; } = Guid.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SectionType TransactionSectionType { get; set; }

       // public TransactionDto Transaction { get; set; } = null; 

        public ICollection<TransactionSectionGroupDto> SectionGroups { get; set; }
        = new List<TransactionSectionGroupDto>();
    }
}
