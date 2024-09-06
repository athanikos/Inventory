namespace Inventory.Products.Contracts.Dto
{
    public class ModifyQuantityDto
    {
        public Guid ProductId { get; set; }   
        public decimal Diff { get; set; }   
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public ModificationType ModificationType { get; set; }  
    }
}