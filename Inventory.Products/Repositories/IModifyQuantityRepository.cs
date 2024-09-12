using Inventory.Products.Contracts.Dto;
using Inventory.Products.Entities;

namespace Inventory.Products.Repositories
{
    public  interface IModifyQuantityRepository
    {
        public Task<QuantityMetric?> GetPreviousWithLockAsync(ModifyQuantityDto dto);

        public  Task<List<QuantityMetric>> GetPostEffectiveDateRowsWithLockAsync(ModifyQuantityDto dto);

        public  Task HasOverlappingRecordsWithLockAsync(ModifyQuantityDto dto);

        public QuantityMetric AddQuantityMetric(Guid productId, decimal value, DateTime effectiveDate);

        public ProductsDbContext Context { get; } 
    }
}
