namespace Inventory.Transactions.Entities
{


    /// <summary>
    /// configuration item
    /// describes the field for an transaction template
    /// </summary>
    public  class Field
    {

        public  Guid Id { get; set; }

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

        public FieldType Type { get; set; }

        public ICollection<Value> FieldValues { get; set; } = new List<Value>();


    }
}
