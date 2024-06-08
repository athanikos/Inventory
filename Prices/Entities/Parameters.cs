namespace Inventory.Prices.Entities
{
    internal class Parameters
    {
        internal Guid Id { get; set; }
        internal string ParameterType { get; set; } = string.Empty;
        internal string Description { get; set; } = string.Empty;
        internal int RunEveryMinutes { get; set; }
        internal string TargetURL { get; set; } = string.Empty;
        internal string TargetKey { get; set; } = string.Empty;
        internal string TargetProductCode { get; set; } = string.Empty;
        internal string TargetPathForProductCode { get; set; } = string.Empty;
        internal Guid MetricId { get; set; }  
    }

}
