using Inventory.Transactions.Contracts;

namespace Inventory.Transactions.Entities.Generic
{


    /// <summary>
    /// configuration item
    /// describes the field for an transaction template
    /// </summary>
    public  class Field
    {

        public  Guid Id { get; set; }

        public Guid SectionId { get; set; }

      //  public Guid TemplateId { get; set; }

        /// <summary>
        /// total price 
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        ///example  unitprice * quantity 
        ///leave empty if not computed from other products 
        /// </summary>
        public string Expression { get; set; } = string.Empty;


        //todo data source , this is for a combo equivalent 
        // the source can be products from inventory possible use labels (categories ) to limit dataset 

    //    public Template Template { get; set; }=null;


        /// <summary>
        /// the type int , decimal datetime 
        /// </summary>
        public FieldType Type { get; set; }

        public ICollection<Value> Values { get; set; } = new List<Value>();


    }
}
