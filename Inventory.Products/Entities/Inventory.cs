
namespace Inventory.Products.Entities
{

    public sealed class Inventory
    {
        public Guid Id { get; set; }
         = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;

        public ICollection<Product> Products
        { get; set; } = new List<Product>();



    }

}