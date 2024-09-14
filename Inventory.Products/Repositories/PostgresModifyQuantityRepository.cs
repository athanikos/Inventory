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
                                                  ORDER BY ""EffectiveDate"" DESC 
                                                  FOR UPDATE NOWAIT"
                                             ) // PostgreSQL: Lock or fail immediately
                                            .AnyAsync())
                throw new ArgumentException(); // todo exception and message 
        
        }


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

     
    }
}
