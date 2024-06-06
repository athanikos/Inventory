using Inventory.Products.Entities;

namespace Inventory.Products.Dto;

public record ProductDto(Guid Id,string Description,string Code, Guid InventoryId, List<ProductMetricDto> Metrics);
 
