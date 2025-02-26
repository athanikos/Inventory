
namespace Inventory.Products.Entities
{

    public sealed class Inventory
    {
        public Guid Id { get; set; }    = Guid.NewGuid();
        public required string Description { get; set; } 
        public required string Code { get; set; }
        public ICollection<Product> Products { get; set; } = [];
        public ICollection<Metric> Metrics { get; set; } = [];

    }

}