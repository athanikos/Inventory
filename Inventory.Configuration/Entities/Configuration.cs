using Inventory.Defaults.Contracts;

namespace Inventory.Defaults.Entities;

public class Configuration
{
    public required ConfigurationType  Type   { get; set; }  
    
    public Guid? EntityId { get; set; }

    public string Value { get; set; }
}





