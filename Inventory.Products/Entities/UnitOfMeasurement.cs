using Inventory.Products.Contracts;

namespace Inventory.Products.Entities;

public class UnitOfMeasurement
{
    public Guid Id { get; set; }
    
    public   string  Text { get; set; }

    public  UnitOfMeasurementType Type { get; set; }
}

