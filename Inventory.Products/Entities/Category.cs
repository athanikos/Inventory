namespace Inventory.Products.Entities
{

    /// <summary>
    /// tree like representation of product categories 
    /// root nodes have FatherId empty 
    /// </summary>
    public class Category
    {
        public Guid Id { get; internal  set; }
            = Guid.NewGuid();

        public string Name { get; internal set; }
            = string.Empty;

        public Guid FatherId { get; internal  set; }
            = Guid.Empty;

        public ICollection<Product> Products { get; private set; }
            = new List<Product>();

    }
}
