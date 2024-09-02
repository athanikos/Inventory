using Inventory.Products.Dto;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Handlers;

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

        List<string> GetDistinctProductCodes(Guid InventoryId);
        List<string> GetDistinctMetricCodes();

        Task AddOrEditProductQuantityMetric(QuantityMetricDto m);
        Task AddOrEditProductMetric(ProductMetricDto m);
        Task<MetricDto> AddMetricAsync(MetricDto dto);
        Task<MetricDto> EditMetricAsync(MetricDto c);
        Task DeleteMetricAsync(MetricDto c);

        Task AddOrEditInventoryMetric(InventoryMetricDto m);
        bool CategoryFatherIdExists(Guid FatherId);

        Task<CategoryDto> AddCategoryAsync(CategoryDto dto);
        bool CategoryIdExists(Guid Id);
        Task<CategoryDto> EditCategoryAsync(CategoryDto c);
        Task DeleteCategoryAsync(CategoryDto c);
        Task<SourceDto> AddSourceAsync(SourceDto sourceDto);

        Task<QuantityMetricDto> LetProduct(LetProductDto dto);    
        ProductMetricDto GetProductMetric(string ProductCode, string MetricCode);

        Task<QuantityMetricDto> AddQuantityMetricAsync(QuantityMetricDto inventoryDto);

        Task<QuantityMetricDto> GetQuantityMetricAsync(QuantityMetricDto inventoryDto);


        void EmptyDB();
    }
}