using System.Text.Json.Serialization;

namespace Inventory.Transactions.Dto
{
    public  class SectionDto
    {
        public Guid Id { get; set; }

        public Guid TemplateId { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<FieldDto> Fields { get; set; } = new();

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType TransactionType { get; set; }
    }
}
