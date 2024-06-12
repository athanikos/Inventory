
namespace Inventory.Products.Contracts.Dto;

public record ProductDto(Guid Id,string Description,string Code, Guid InventoryId, List<ProductMetricDto> Metrics);
 
