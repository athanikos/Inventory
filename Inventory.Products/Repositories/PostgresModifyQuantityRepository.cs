using Inventory.Products.Contracts.Dto;
using Inventory.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Products.Repositories
{
    /// <summary>
    /// todo move buinsess code to service this does not seem to be repo code
    /// </summary>
    public  class PostgresModifyQuantityRepository : IModifyQuantityRepository
    {
        private ProductsDbContext _context;
        public ProductsDbContext Context { get => _context; set => _context = value; }
        
        public PostgresModifyQuantityRepository(ProductsDbContext context) => Context = context;

        public async Task<QuantityMetric?> GetPreviousWithLockAsync(ModifyQuantityDto dto)
        {
            return  await  Context.QuantityMetrics
                                            .FromSql
                                             (
                                                 $@"
                                                  SELECT ""ProductId"", 
                                                  ""EffectiveDate"",  ""Value"" , 
                                                  ""Diff"", ""IsCancelled"", ""TransactionId""
                                                  FROM ""Products"".""QuantityMetric"" 
                                                  WHERE ""ProductId"" = {dto.ProductId}
                                                  AND ""EffectiveDate""  < {dto.EffectiveFrom}
                                                  AND ""IsCancelled"" IS NOT TRUE 
                                                  ORDER BY ""EffectiveDate"" DESC 
                                                  FOR UPDATE NOWAIT"
                                             ) // PostgreSQL: Lock or fail immediately
                                            .FirstOrDefaultAsync();
     
        }



        
        public async Task<List<QuantityMetric>> GetPostEffectiveDateRowsWithLockAsync(ModifyQuantityDto dto)
        {
                   return await Context
                                            .QuantityMetrics
                                            .FromSql
                                             (
                                                 $@"
                                                  SELECT ""ProductId"", 
                                                  ""EffectiveDate"",  ""Value"" ,
                                                  ""Diff"", ""IsCancelled"", ""TransactionId""
                                                  FROM ""Products"".""QuantityMetric"" 
                                                  WHERE ""ProductId"" = {dto.ProductId}
                                                  AND ""EffectiveDate""  > {dto.EffectiveFrom}
                                                  AND ""IsCancelled"" IS NOT TRUE 
                                                  ORDER BY ""EffectiveDate"" ASC 
                                                  FOR UPDATE NOWAIT"
                                             ) // PostgreSQL: Lock or fail immediately
                                            .ToListAsync();

        }

        public  async   Task HasOverlappingRecordsWithLockAsync(ModifyQuantityDto dto )
        {
            if (dto.ModificationType == Contracts.ModificationType.Buy)
                return;



            if (await Context
                                            .QuantityMetrics
                                            .FromSql
                                             (
                                                 $@"
                                                  SELECT ""ProductId"", 
                                                  ""EffectiveDate"",  ""Value"" ,
                                                  ""Diff"", ""IsCancelled"", ""TransactionId""
                                                  FROM ""Products"".""QuantityMetric"" 
                                                  WHERE ""ProductId"" = {dto.ProductId}
                                                  AND ""EffectiveDate""  < {dto.EffectiveTo}
                                                  AND ""EffectiveDate""  > {dto.EffectiveFrom}
                                                  AND ""Value""  - {dto.Diff} < 0 
                                                  AND ""IsCancelled"" IS NOT TRUE 
                                                  ORDER BY ""EffectiveDate"" DESC 
                                                  FOR UPDATE NOWAIT"
                                             ) // PostgreSQL: Lock or fail immediately
                                            .AnyAsync())
                throw new ArgumentException(); // todo exception and message 
        
        }

        /// <summary>
        /// todo move to repo 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="value"></param>
        /// <param name="effectiveDate"></param>
        /// <returns></returns>
        public QuantityMetric  AddQuantityMetric(Guid productId, decimal value, DateTime effectiveDate )
        {
            QuantityMetric qmStart = new QuantityMetric()
            {
                ProductId = productId,
                Value = value, 
                EffectiveDate = effectiveDate
            };
             Context.QuantityMetrics.Add(qmStart);
            return qmStart; 
        }

        public QuantityMetric EditQuantityMetric(Guid productId,  DateTime effectiveDate, bool IsCancelled)
        {
            QuantityMetric qmStart = new QuantityMetric(productId, effectiveDate, IsCancelled);
            Context.QuantityMetrics.Attach(qmStart);
            return qmStart;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
