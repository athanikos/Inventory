namespace Inventory.Products.Entities
{
    /// <summary>
    /// A source of metrics 
    /// </summary>
    public class Source
    {
        public Guid Id { get; set; }
            = Guid.NewGuid();

        public string Description { get; set; }
            = string.Empty;

      
        public ICollection<Metric> Metrics
        { get; set; } = new List<Metric>();


    }


}
