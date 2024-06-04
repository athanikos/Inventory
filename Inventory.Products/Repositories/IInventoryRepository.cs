using Inventory.Products.Dto;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Inventory.Products.Repositories
{
    public interface IInventoryRepository
    {
        Task<InventoryDto> AddInventoryAsync(InventoryDto dto);
        bool InventoryFatherIdExists(Guid FatherId);
        Task<InventoryDto> EditInventoryAsync(InventoryDto c);
        Task DeleteInventoryAsync(InventoryDto c);

        Task<ProductDto> AddProductAsync(ProductDto dto);
        Task<ProductDto> EditProductAsync(ProductDto c);
        Task DeleteProductAsync(ProductDto c);

        Task<MetricDto> AddMetricAsync(MetricDto dto);
        Task<MetricDto> EditMetricAsync(MetricDto c);
        Task DeleteMetricAsync(MetricDto c);

        Task<CategoryDto> AddCategoryAsync(CategoryDto dto);
        bool CategoryFatherIdExists(Guid FatherId);
        Task<CategoryDto> EditCategoryAsync(CategoryDto c);
        Task DeleteCategoryAsync(CategoryDto c);


    }
}