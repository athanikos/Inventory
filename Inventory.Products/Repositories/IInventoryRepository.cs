using Inventory.Products.Dto;
using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Repositories
{
    public interface IInventoryRepository
    {
        Task<InventoryDto> AddInventoryAsync(InventoryDto dto);
        bool InventoryIdExists(Guid Id);
        Task<InventoryDto> EditInventoryAsync(InventoryDto c);
        Task DeleteInventoryAsync(InventoryDto c);

        Task<ProductDto> AddProductAsync(ProductDto dto);
        Task<ProductDto> EditProductAsync(ProductDto c);
        Task DeleteProductAsync(ProductDto c);
        bool ProductDescriptionOrCategoryIsUsed(ProductDto c);

        Task AddOrEditProductMetric(ProductMetricDto m);


        Task<MetricDto> AddMetricAsync(MetricDto dto);
        Task<MetricDto> EditMetricAsync(MetricDto c);
        Task DeleteMetricAsync(MetricDto c);

        Task<CategoryDto> AddCategoryAsync(CategoryDto dto);
        bool CategoryIdExists(Guid Id);
        Task<CategoryDto> EditCategoryAsync(CategoryDto c);
        Task DeleteCategoryAsync(CategoryDto c);
        Task<SourceDto> AddSourceAsync(SourceDto sourceDto);
    }
}