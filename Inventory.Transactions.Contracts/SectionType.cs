namespace Inventory.Transactions.Contracts
{
    /// <summary>
    /// Aggregate :  this is where sum of other section values are stored, just products or anything  
    /// Product :  each line is bound to a productId, also marks the section as Buy/Sell/Let 
    /// Entity : No multiple lines just fields i.e. Individual who rents or corporate 
    /// 
    /// </summary>
    public enum SectionType
    {
        ProductBuy,        
        ProductSell,
        ProductLet,
        IndividualEntity,
        CorporateEntity,
        Aggregate  
    }
}
