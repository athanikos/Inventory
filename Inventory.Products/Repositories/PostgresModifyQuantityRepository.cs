using FastEndpoints;
using FluentValidation.Validators;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using System.ComponentModel;
using System.Transactions;

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




        //public async Task  SaveAndCommit(IDbContextTransaction transaction)
        //{

        //}

        //public async Task Rollback(IDbContextTransaction transaction)
        //{
        //    await transaction.RollbackAsync();
        //    Context.ChangeTracker.Clear();
        //}


        /// <summary>
        /// attempts to find previous entry per productId and effective date 
        /// if found it adds dif to value and adds to context 
        /// if not found throws Argument Exception 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<QuantityMetric?> GetPreviousWithLockAsync(ModifyQuantityDto dto)
        {
            return  await                    Context
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
     
        }

        private void Unlet(ModifyQuantityDto dto, QuantityMetric qmStart)
        {
            if (dto.ModificationType != Contracts.ModificationType.Let)
                return;

            QuantityMetric qmEnd = new QuantityMetric()
                {
                    ProductId = dto.ProductId,
                    Value = qmStart.Value + dto.Diff,
                    EffectiveDate = dto.EffectiveTo.AddDays(1) // todo parametrize increment this is daily !!!!
                };
                Context.QuantityMetrics.Add(qmEnd);
        }

        
        public async Task<List<QuantityMetric>> GetPostEffectiveDateRowsWithLockAsync(ModifyQuantityDto dto)
        {
                   return await Context
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
                                                  ""EffectiveDate"",  ""Value"" 
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


        public void AddQuantityMetric(Guid productId, decimal value, DateTime effectiveDate )
        {
            QuantityMetric qmStart = new QuantityMetric()
            {
                ProductId = productId,
                Value = value, 
                EffectiveDate = effectiveDate
            };
             Context.QuantityMetrics.Add(qmStart);
        }

     
    }
}
