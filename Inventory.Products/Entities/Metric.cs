namespace Inventory.Products.Entities
{
    public sealed class
        Metric
    {

        public Guid Id { get; internal set; }
            = Guid.NewGuid();

        public string Description { get; internal set; }
            = string.Empty;



        public string Code { get; internal set; }
            = string.Empty;


        public ICollection<Product> Products { get; internal set; }
            = new List<Product>();


        public ICollection<Inventory> Inventories  { get; internal set; }
    = new List<Inventory>();

        /// <summary>
        /// System the attribute value came from 
        /// </summary>
        public Guid SourceId { get; set; }
    }
}
