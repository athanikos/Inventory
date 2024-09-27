namespace Inventory.Prices.Entities
{
 
    public  class PricesParameter
    {
        public Guid Id { get; set; }
        public string ParameterType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RunEveryMinutes { get; set; }
        public string TargetURL { get; set; } = string.Empty;
        public string TargetKey { get; set; } = string.Empty;
        public string TargetProductCode { get; set; } = string.Empty;
        public string TargetPathForProductCode { get; set; } = string.Empty;
        public string TargetCurrency { get; set; } = string.Empty;
        public Guid MetricId { get; set; }
        public Guid ProductId { get; set; } 
    }

}
