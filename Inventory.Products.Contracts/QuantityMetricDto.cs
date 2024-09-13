namespace Inventory.Products.Contracts.Dto
{

    public class QuantityMetricDto
    {
        public QuantityMetricDto(
                                Guid productId, 
                                decimal value, 
                                DateTime effectiveDate ,
                                Guid transactionId,
                                decimal  diff,
                                bool  isCancelled 
                                )
        {
            ProductId = productId;
            Value = value;
            EffectiveDate = effectiveDate;
            TransactionId = transactionId;
            Diff = diff;    
            IsCancelled = isCancelled;  
        }

        public static QuantityMetricDto NewQuantityMetricDto(
                                        Guid productId, decimal value,
                                        DateTime effectiveDate,
                                        Guid transactionId,
                                        decimal diff,
                                        bool isCancelled)
        {
            return new QuantityMetricDto(productId, 
                                         value,
                                         effectiveDate,  
                                         transactionId,
                                         diff,
                                         isCancelled);
        }

        public Guid ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public decimal Value { get;  set; }
        public DateTime EffectiveDate { get; set; }  = DateTime.MinValue;
        public Guid TransactionId { get; set; }
        public decimal Diff { get; set; }
        public bool IsCancelled { get; set; }


        public override string ToString()
        {
            return nameof(ProductId) +  ProductId.ToString() + " " +
                   Value + " "  +  EffectiveDate;
        }
    }

}