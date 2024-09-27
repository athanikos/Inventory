namespace Inventory.Products.Services;

/// <summary>
/// describes the service that provides
/// billing information for an interval
/// </summary>
public interface IRatesService
{
     void GetRatesForIntervals(List<ProductInterval> productIntervals);
    
}

public  record ProductInterval(DateTime From, DateTime To, Guid ProductId);

public  record InventoryInterval(DateTime From, DateTime To, Guid InventoryId);

