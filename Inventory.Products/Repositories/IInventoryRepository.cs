using Inventory.Products.Contracts;
using Inventory.Products.Dto;
using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Repositories
{
    public interface IInventoryRepository
    {
        Task<InventoryDto> AddInventoryAsync(InventoryDto dto);

        Task<InventoryDto> GetInventoryAsync(Guid inventoryId);
        
        bool InventoryIdExists(Guid id);
        Task<InventoryDto> EditInventoryAsync(InventoryDto c);
        Task DeleteInventoryAsync(InventoryDto c);

        Task<ProductDto> AddProductAsync(ProductDto dto);
        Task<ProductDto> EditProductAsync(ProductDto c);
        Task DeleteProductAsync(ProductDto c);
        Task<bool> ProductDescriptionOrCategoryIsUsedAsync(ProductDto c);

        List<string> GetDistinctProductCodes(Guid inventoryId);
        List<string> GetDistinctMetricCodes();
        Task AddOrEditProductMetricAsync(ProductMetricDto m);
        Task<ProductMetricDto> GetProductMetricAsync(Guid productId, DateTime effectiveDate    );

        Task<MetricDto> AddMetricAsync(MetricDto dto);
        Task<MetricDto> EditMetricAsync(MetricDto c);
        Task DeleteMetricAsync(MetricDto c);

        Task AddOrEditInventoryMetric(InventoryMetricDto m);
        Task<bool> CategoryFatherIdExistsAsync(Guid fatherId);

        Task<CategoryDto> AddCategoryAsync(CategoryDto dto);
        Task<bool> CategoryIdExistsAsync(Guid id);
        Task<CategoryDto> EditCategoryAsync(CategoryDto c);
        Task DeleteCategoryAsync(CategoryDto c);
        Task<SourceDto> AddSourceAsync(SourceDto sourceDto);

        Task<ProductMetricDto> GetProductMetricAsync(string productCode, string metricCode);
        Task<QuantityMetricDto> AddQuantityMetricAsync(QuantityMetricDto dto);
        Task<QuantityMetricDto> EditQuantityMetricAsync(QuantityMetricDto dto);
        Task<QuantityMetricDto> GetQuantityMetricAsync(Guid productId, DateTime effectiveDate);
        void  AddQuantityMetric(QuantityMetricDto dto);
        
        Task<int> SaveChangesAsync();
        void EmptyDb();

        public  Task<List<QuantityMetricDto>>    CancellQuantityMetricsAsync(Guid transactionId);
   
        public Task<List<QuantityMetricDto>>     GetQuantityMetricsAsync(Guid transactionId);

        public  Task<List<QuantityMetricDto>> GetQuantityMetricsAsync();

        public ProductsDbContext Context();

        public Task<UnitOfMeasurementDto> AddUnitOfMeasurementAsync(UnitOfMeasurementDto dto);

         
        public Task<UnitOfMeasurementDto> GetUnitOfMeasurementAsync(Guid id);

    }
}