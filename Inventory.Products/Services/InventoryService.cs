using System.Diagnostics;
using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;

namespace Inventory.Products.Services;



public class InventoryService(IInventoryRepository repo) : IInventoryService
{
    public async Task<CategoryDto> AddCategoryAsync(CategoryDto dto)
    {
        if (await repo.CategoryIdExistsAsync(dto.FatherId))
            return await repo.AddCategoryAsync(new CategoryDto(Guid.NewGuid(), dto.Description, dto.FatherId));
        throw new Exception("Father Id does not exist");

    }

    public async Task< InventoryDto> AddInventoryAsync(InventoryDto dto)
    {
        return await repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), dto.Description));
    }

    public async Task<ProductDto> AddProductAsync(ProductDto dto)
    {
        return await repo.AddProductAsync(
                new ProductDto(Guid.Empty, dto.Description, dto.Code, 
                    dto.InventoryId, dto.Metrics));
    }

    public async Task<ProductDto> EditProductAsync(ProductDto dto)
    {
        return await repo.EditProductAsync(new ProductDto(dto.Id, dto.Description,
            dto.Code,dto.InventoryId, dto.Metrics));
    }

    public async Task<InventoryDto> EditInventoryAsync(InventoryDto dto)
    {
        return await repo.EditInventoryAsync(new InventoryDto(dto.Id, dto.Description));
    }
    
    
    
    public async Task<MetricDto> AddMetricAsync(MetricDto dto)
    {
        return await repo.AddMetricAsync(new MetricDto
            (dto.Id, dto.Description, dto.Code, dto.SourceId));
    }


    public async Task<MetricDto> EditMetricAsync(MetricDto dto)
    {
        return await repo.EditMetricAsync(new MetricDto
            (dto.Id, dto.Description, dto.Code, dto.SourceId));

    }

    public async Task<QuantityMetricDto> AddQuantityMetricAsync(QuantityMetricDto dto)
    {
        return await repo.AddQuantityMetricAsync(new QuantityMetricDto
            (dto.ProductId,dto.Value, dto.EffectiveDate, dto.TransactionId, dto.Diff, dto.IsCancelled));

    }

    public async Task<SourceDto> AddSourceAsync(SourceDto dto)
    {
        return await repo.AddSourceAsync(new SourceDto(Guid.Empty,dto.Description));
    }
    
    public async Task<CategoryDto> EditCategoryAsync(CategoryDto dto)
    {
        return await repo.AddCategoryAsync(new CategoryDto(dto.Id,dto.Description,dto.FatherId));
    }

 


   
}

