namespace Inventory.Products.Handlers
{
    public class LetProductDto
    {
        public Guid ProductId { get; set; }   
        /// <summary>
        /// negative for decrease
        /// </summary>
        public decimal IncreaseBy { get; set; }       
        
        public decimal DecreaseBy { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }

    }
}