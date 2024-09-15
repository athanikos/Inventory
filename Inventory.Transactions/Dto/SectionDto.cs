using Inventory.Transactions.Contracts;
using System.Text.Json.Serialization;

namespace Inventory.Transactions.Dto
{
    public  class SectionDto
    {

        public SectionDto(Guid id) { this.Id = Id; }
        public SectionDto() {  }


        public Guid Id { get; set; } = Guid.Empty;

        public Guid TemplateId { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<FieldDto> Fields { get; set; } = [];

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SectionType SectionType { get; set; }
    }
}
