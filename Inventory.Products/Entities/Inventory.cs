
namespace Inventory.Products.Entities
{

    public sealed class Inventory
    {
        public Guid Id { get; set; }
         = Guid.NewGuid();
        public string Description { get; set; } 
        public string Code { get; set; }
        public ICollection<Product> Products { get; set; } = [];
        public ICollection<Metric> Metrics { get; set; } = [];

    }

}