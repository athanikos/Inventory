using Inventory.Transactions.Entities;

namespace Inventory.Transactions.Dto
{
    public  class FieldDto
    {
        public Guid Id { get; set; }

        public Guid TemplateId { get; set; }
        /// <summary>
        /// total price 
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        ///example  unitprice * quantity 
        /// </summary>
        public string Expression { get; set; } = string.Empty;

        public FieldType Type { get; set; }

    }

}
