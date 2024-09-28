namespace Inventory.Defaults.Entities;

public class Default
{
    
    public required string  Type   { get; set; } // the enum string value to upper CURRENCY / STORE ... // add unique constraint 

    public required int  Value { get; set; } // the enum int equivalent value
    
    public Guid? EntityId { get; set; } // the id of the row in the corresponding entity table, for example for Euro it will hold  the UnitOfMeasurement Guid for euro 

    public string? EntityName { get; set; } // UnitOfMeasurement ...  needs to be resolved with reflection 

    
}





