
using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Contracts;

public class ProductDto
{ 
    public  ProductDto(Guid id,
    string description,
    string code, 
    Guid inventoryId,
    List<ProductMetricDto> metrics)
    {
        Id = id;
        Description = description;
        Code = code;
        InventoryId = inventoryId;
        Metrics = metrics;
    }
    public ProductDto(Guid id)
    {
        Id = id;
    }

    public Guid Id { get;  set; } 
    public string Description { get; set; } 
    public string Code { get; private set; } 
    public Guid InventoryId { get; private set; } 
    public List<ProductMetricDto> Metrics { get; private set; } 

    public  static ProductDto NewProductDto(Guid inventoryId, string productCode)
    {
        return new ProductDto(Guid.NewGuid(),"",
                                     productCode,
                                        inventoryId,
                                     new List<ProductMetricDto>());
    }
}
