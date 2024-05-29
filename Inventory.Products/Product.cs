namespace Inventory.Products
{
    public class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Description { get; private set; } = string.Empty;

        public ProductCategory? ProductCategory { get; private set; }

        public ICollection<ProductMetric> ProductMetrics { get; private set; } = new List<ProductMetric>();

    }


}
