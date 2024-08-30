using Inventory.Transactions.Entities;
using System.Text.Json.Serialization;

namespace Inventory.Transactions.Dto
{
    public  class FieldDto
    {
        public FieldDto(Guid Id)
        {
            this.Id = Id;   
        }
        public FieldDto() { }

        public Guid Id { get; set; } = Guid.Empty;

        public Guid SectionId { get; set; }

        public Guid TemplateId { get; set; }
        /// <summary>
        /// total price 
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        ///example  unitprice * quantity 
        /// </summary>
        public string Expression { get; set; } = string.Empty;


        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FieldType Type { get; set; }
    }



}
