namespace Inventory.Products.Entities
{
   // todo remove ?
   
    public class InBoundQuantityMetric : QuantityMetric
    {
        public RecordSource Source { get; set; }
        public bool IsNew { get; set; }

        public InBoundQuantityMetric(Guid productId, decimal value,
            DateTime effectiveDate, bool isNew, RecordSource source )
        {
            ProductId = productId; 
            Value = value;
            EffectiveDate = effectiveDate; 
            IsNew = isNew; 
            Source = source;   
        }
    }

    public enum RecordSource
    {
        Store = 0,
        InBound = 1
    }
}
