﻿using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Entities;

namespace Inventory.Products.Repositories
{
    public interface IModifyQuantityRepository
    {
        public Task<QuantityMetric?> GetPreviousWithLockAsync(ModifyQuantityDto dto);

        public Task<List<QuantityMetric>> GetPostEffectiveDateRowsWithLockAsync(ModifyQuantityDto dto);

        public Task HasOverlappingRecordsWithLockAsync(ModifyQuantityDto dto);

        public QuantityMetric AddQuantityMetric(Guid productId, decimal value, DateTime effectiveDate, decimal diff, ModificationType type  );

        public QuantityMetric EditQuantityMetric(Guid productId, DateTime effectiveDate, bool isCancelled);

        public Task SaveChangesAsync();

        public Task<List<ModifyQuantityDto>> GetQuantityMetricsPostEffectiveDate(Guid productId, DateTime minimumEffectiveDate);

        public ProductsDbContext Context { get; }

    }
}
