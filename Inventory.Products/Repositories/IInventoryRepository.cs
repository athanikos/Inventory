using Inventory.Products.Contracts;
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
        Task<bool> ProductDescriptionOrCategoryIsUsedAsync(ProductDto c);

        List<string> GetDistinctProductCodes(Guid InventoryId);
        List<string> GetDistinctMetricCodes();
        Task AddOrEditProductMetricAsync(ProductMetricDto m);
        Task<ProductMetricDto> GetProductMetricAsync(Guid ProductId, DateTime effectivedate    );

        Task<MetricDto> AddMetricAsync(MetricDto dto);
        Task<MetricDto> EditMetricAsync(MetricDto c);
        Task DeleteMetricAsync(MetricDto c);

        Task AddOrEditInventoryMetric(InventoryMetricDto m);
        Task<bool> CategoryFatherIdExistsAsync(Guid FatherId);

        Task<CategoryDto> AddCategoryAsync(CategoryDto dto);
        Task<bool> CategoryIdExistsAsync(Guid Id);
        Task<CategoryDto> EditCategoryAsync(CategoryDto c);
        Task DeleteCategoryAsync(CategoryDto c);
        Task<SourceDto> AddSourceAsync(SourceDto sourceDto);

        Task<ProductMetricDto> GetProductMetricAsync(string ProductCode, string MetricCode);
        Task<QuantityMetricDto> AddQuantityMetricAsync(QuantityMetricDto dto);
        Task<QuantityMetricDto> EditQuantityMetricAsync(QuantityMetricDto dto);
        Task<QuantityMetricDto> GetQuantityMetricAsync(Guid ProductId, DateTime EffectiveDate);
        void  AddQuantityMetric(QuantityMetricDto dto);
        
        Task<int> SaveChangesAsync();
        void EmptyDB();

        public  Task<List<QuantityMetricDto>>    CancellQuantityMetricsAsync(Guid TransactionId);
   
        public Task<List<QuantityMetricDto>>     GetQuantityMetricsAsync(Guid TransactionId);

        public  Task<List<QuantityMetricDto>> GetQuantityMetricsAsync();


    }
}