namespace Inventory.Products.Entities
{
    public class Product
    {
        public Guid Id { get;  set; }         = Guid.NewGuid();

        public string Description { get;  set; }      = string.Empty;

        public string Code { get; set; }        = string.Empty;

        public ICollection<Category> Categories { get;  set; } = new List<Category>();

        public ICollection<Metric> Metrics   { get;  set; } = new List<Metric>();

        public Guid InventoryId { get; set; }
    }


}
