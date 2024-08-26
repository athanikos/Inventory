
namespace Inventory.Transactions.Entities
{
    public class TransactionItem
    {
        public Guid Id { get; set; }

        /// <summary>
        /// the header entity 
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// the product id for which this item applies 
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        ///  the template to which this item refers to 
        /// </summary>
        public Guid TemplateId { get; set; }    
        
        /// <summary>
        /// the actual values of fields 
        /// </summary>
        public required ICollection<TransactionItemFieldValue> FieldValues { get; set; } 
           


      
    }
}