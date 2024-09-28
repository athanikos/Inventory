using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;

namespace Inventory.Products.Services;

public interface  IInventoryService
{
    Task<CategoryDto> AddCategoryAsync(CategoryDto dto);
    Task<InventoryDto> AddInventoryAsync(InventoryDto dto);
    
    Task<ProductDto> AddProductAsync(ProductDto dto);
    
    Task< ProductDto >  EditProductAsync(ProductDto dto);

    
    Task<MetricDto>  AddMetricAsync(MetricDto dto);

    Task<QuantityMetricDto>  AddQuantityMetricAsync(QuantityMetricDto dto);
    Task<SourceDto>  AddSourceAsync(SourceDto dto);
    
    Task< CategoryDto>  EditCategoryAsync(CategoryDto dto);
    
    Task< MetricDto>    EditMetricAsync(MetricDto dto);
  
    
    Task<UnitOfMeasurementDto> AddUnitOfMeasurementAsync(UnitOfMeasurementDto dto);
    Task<UnitOfMeasurementDto> EditUnitOfMeasurementAsync(UnitOfMeasurementDto dto);

    Task InitializeDefaults();

}