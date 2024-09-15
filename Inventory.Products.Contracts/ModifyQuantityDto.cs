namespace Inventory.Products.Contracts.Dto
{
    public class ModifyQuantityDto
    {
        public Guid ProductId { get; set; }
        public decimal Value;
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public ModificationType ModificationType { get; set; }

        public Guid TransactionId { get; set; }
        public decimal Diff { get; set; }
        public bool IsCancelled { get; set; } = false;  
    }

}