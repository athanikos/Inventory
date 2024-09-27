namespace Inventory.Products.Entities
{
   // todo remove ?
   
    public class InBoundQuantityMetric : QuantityMetric
    {
        public RecordSource Source { get; set; }
        public bool IsNew { get; set; }

        public InBoundQuantityMetric(Guid ProductId, decimal Value,DateTime EffectiveDate, bool IsNew, RecordSource Source           )
        {
            this.ProductId = ProductId; 
            this.Value = Value;
            this.EffectiveDate = EffectiveDate; 
            this.IsNew = IsNew; 
            this.Source = Source;   
        }
      

    }

    public enum RecordSource
    {
        Store = 0,
        InBound = 1
    }
}
