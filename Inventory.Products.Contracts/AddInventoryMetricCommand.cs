using Inventory.Products.Contracts.Dto;
using MediatR;

namespace Inventory.Products.Contracts
{
    public class AddInventoryMetricCommand : IRequest<InventoryMetricDto>
    {
        public Guid InventoryId { get; set; }

        public Guid MetricId { get; set; }

        public decimal Value { get; set; }

        public DateTime EffectiveDate { get; set; } = DateTime.MinValue;

        public string Currency { get; set; } = string.Empty;


        public AddInventoryMetricCommand(Guid InventoryId, Guid MetricId, decimal Value, 
            DateTime EffectiveDate, string Curtrency)
        {
            this.InventoryId = InventoryId;
            this.MetricId = MetricId;
            this.Value = Value;
            this.EffectiveDate = EffectiveDate;
            this.Currency = Curtrency;
        }


    }
}
