
namespace Inventory.Products.Entities
{
    public class TransactionItem
    {
        public Guid Id { get; set; }

        public Guid TransactionId { get; set; }

        public Guid ProductId { get; set; }

        public string Description { get; set; } = string.Empty; 

        public string TransactionType { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal VatPercentage { get; set; } =0;

        public decimal PriceAfterVat { get; set; } 

        public decimal Discount { get; set; } =0;

        public decimal DiscountAmount { get; set; } = 0;

        public decimal TransactionFees { get; set; } = 0;

        public decimal DeliveryFees { get; set; } = 0;

        public decimal FinalPrice { get; set; } 
    }
}