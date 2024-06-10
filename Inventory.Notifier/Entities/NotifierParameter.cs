namespace Inventory.Prices.Entities
{

    public  class NotifierParameter
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int RunEveryMinutes { get; set; }
        public Guid MetricId { get; set; }
        public Guid ProductId { get; set; } 
    }

}
