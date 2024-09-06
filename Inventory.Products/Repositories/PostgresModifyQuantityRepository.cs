using Inventory.Products.Contracts.Dto;
using Inventory.Products.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Inventory.Products.Repositories
{
    public  class PostgresModifyQuantityRepository : IModifyQuantityRepository
    {
        private ProductsDbContext _context; 

        public PostgresModifyQuantityRepository(ProductsDbContext context )
        {
            _context = context;     
        }

        /// <summary>
        /// attempts to find previous entry per productId and effective date 
        /// if found it adds dif to value and adds to context 
        /// if not found throws Argument Exception 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task AddBasedOnPrevious(ModifyQuantityDto dto)
        {
            var previousInTimeEntry = await _context
                                            .QuantityMetrics
                                            .FromSql
                                             (
                                                 $@"
                                                  SELECT ""ProductId"", 
                                                  ""EffectiveDate"",  ""Value"" 
                                                  FROM ""Products"".""QuantityMetric"" 
                                                  WHERE ""ProductId"" = {dto.ProductId}
                                                  AND ""EffectiveDate""  < {dto.EffectiveFrom}
                                                  ORDER BY ""EffectiveDate"" DESC 
                                                  FOR UPDATE NOWAIT"
                                             ) // PostgreSQL: Lock or fail immediately
                                            .FirstOrDefaultAsync();

            if (dto.ModificationType!=Contracts.ModificationType.Buy)
                   if (previousInTimeEntry == null)
                        throw new ArgumentException($"no previous entry found with less than {dto.EffectiveFrom} ");

            QuantityMetric qm = new QuantityMetric()
            {
                ProductId = dto.ProductId,
                Value = previousInTimeEntry == null ? 0:  previousInTimeEntry.Value,
                EffectiveDate = dto.EffectiveFrom
             };
            qm.Value = ModifyQuantity(dto, qm);
            _context.QuantityMetrics.Add(qm);

        }

        /// <summary>
        /// iterates through all records post effective date 
        /// and updates quantity 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ModifyQuantityPostEffectiveDate(ModifyQuantityDto dto)
        {
            if (dto.ModificationType == Contracts.ModificationType.Let)
                return;

            var postEffectiveDateRows = await _context
                                            .QuantityMetrics
                                            .FromSql
                                             (
                                                 $@"
                                                  SELECT ""ProductId"", 
                                                  ""EffectiveDate"",  ""Value"" 
                                                  FROM ""Products"".""QuantityMetric"" 
                                                  WHERE ""ProductId"" = {dto.ProductId}
                                                  AND ""EffectiveDate""  > {dto.EffectiveFrom}
                                                  ORDER BY ""EffectiveDate"" ASC 
                                                  FOR UPDATE NOWAIT"
                                             ) // PostgreSQL: Lock or fail immediately
                                            .ToListAsync();

            foreach (var item in postEffectiveDateRows)
            {
                 item.Value = ModifyQuantity(dto, item);
            }
        }

        private static decimal ModifyQuantity(ModifyQuantityDto dto, QuantityMetric item)
        {
            decimal newValue = 0;

            if (dto.ModificationType == Contracts.ModificationType.Buy)
                newValue = item.Value + dto.Diff;
            else if (dto.ModificationType == Contracts.ModificationType.Sell)
                newValue = item.Value - dto.Diff;
            return newValue;
        }

        /// <summary>
        /// https://www.milanjovanovic.tech/blog/a-clever-way-to-implement-pessimistic-locking-in-ef-core
        /// </summary>
        public async Task ModifyQuantityMetrics(List<ModifyQuantityDto> inboundQuantities)
        {
            await using var transaction = await _context.Database
                .BeginTransactionAsync();
            
            
            try
            {
                foreach (var item in inboundQuantities)
                {
                        await AddBasedOnPrevious(item);
                        await ModifyQuantityPostEffectiveDate(item); 
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
            }

               
        }
    }
}
