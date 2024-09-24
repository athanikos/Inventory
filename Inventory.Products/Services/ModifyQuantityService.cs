using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Entities;
using Inventory.Products.Repositories;
using Microsoft.Extensions.Logging;

namespace Inventory.Products.Services
{
    /// <summary>
    ///  Implements quantity increment / decrement with locks in quantity metric entity  
    ///  Validates inbound intervals against store entries
    ///  Validates inbound entries aginst each other 
    /// </summary>
    public class ModifyQuantityService(IModifyQuantityRepository repo) : IModifyQuantityService
    {
        private readonly IModifyQuantityRepository _repo = repo;
        private readonly ModifyQuantityInterval _interval = ModifyQuantityInterval.Daily;

        public enum ModifyQuantityInterval
        {
            Daily = 0 ,
            Hourly = 1 ,    
            Minutely = 2 ,  
        }

        private static void  Validate(List<ModifyQuantityDto> inboundQuantities)
        {
            if  (inboundQuantities.Any(o => o.Value < 0))
                throw new InvalidDiffException();


            if (inboundQuantities.Any(o => o.Diff < 0))
                throw new InvalidDiffException();

        }


        public async Task CancelQuantityMetricsAsync(List<ModifyQuantityDto> inboundQuantities)
        {
            Validate(inboundQuantities);

            await using var transaction = await _repo.Context.Database.BeginTransactionAsync();
            try
            {
                // cancell all inbound records 
                foreach (var item  in inboundQuantities)
                    _repo.EditQuantityMetric(item.ProductId, item.EffectiveFrom, true);
                await _repo.SaveChangesAsync();

                // group by productId 
                var groupedInboundQuantities = from i in inboundQuantities
                                               where i.ModificationType.IsBuyOrSell()
                                               group i by i.ProductId into g
                                               select new ModifyQuantityDto
                                                            { ProductId = g.Key, 
                                                              EffectiveFrom = g.Min(o=>o.EffectiveFrom),
                                                              EffectiveTo = g.Max(o=>o.EffectiveTo),

                                                             };

                foreach (var item in groupedInboundQuantities)
                {
                    var previousItem = await _repo.GetPreviousWithLockAsync(item);
                    if (previousItem == null)
                        break; 

                    var qm = new ModifyQuantityDto()
                    {
                        ProductId = previousItem.ProductId,
                        EffectiveFrom = previousItem.EffectiveDate,
                        EffectiveTo = previousItem.EffectiveDate,
                        Diff = previousItem.Diff,
                        IsCancelled = previousItem.IsCancelled,
                        Value = previousItem.Value, 
                        ModificationType = previousItem.ModificationType,
                    };

                    await ModifyQuantityPostEffectiveDate(qm);


                }

                 
                await _repo.Context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception )
            {
                await transaction.RollbackAsync();
                _repo.Context.ChangeTracker.Clear();
            }

        }

            /// <summary>
            ///  Given a list of modifications on product's quantity 
            ///  opens a transaction, locks all quantity metrics per productId 
            ///  and attempts to add the new records by adding / subtracting quantities 
            ///  if there are entities after it updates all records post effective date 
            ///  the transaction will fail if quantity is less than 0 
            /// </summary>
            /// <param name="inboundQuantities"></param>
            /// <returns></returns>
        public async Task ModifyQuantityMetricsAsync(List<ModifyQuantityDto> inboundQuantities)
        {
            Validate(inboundQuantities);
            inboundQuantities = [.. inboundQuantities.OrderBy(o => o.EffectiveFrom)];
            await using var transaction = await _repo.Context.Database.BeginTransactionAsync();
            try
            {
              
                foreach (var item in inboundQuantities)
                      await AddWithUpdatedQuantityBasedOnPrevious(item);    
                await _repo.Context.SaveChangesAsync();

                var DistinctProductIdsWithMinimumEffective = (
                                                             from prod in inboundQuantities
                                                             group prod by prod.ProductId into g
                                                             select new ModifyQuantityDto
                                                             {
                                                                 ProductId = g.Key,
                                                                 EffectiveFrom = g.Min(o => o.EffectiveFrom),
                                                             }).ToList();
           
                List<ModifyQuantityDto> refetchedEntries = [];
                foreach (var prod in DistinctProductIdsWithMinimumEffective)
                {
                    refetchedEntries.AddRange(
                               await _repo.GetQuantityMetricsPostEffectiveDate(prod.ProductId, prod.EffectiveFrom)
                        );
                }

                foreach (var prod in refetchedEntries)
                {
                    await ModifyQuantityPostEffectiveDate(prod);
                    await _repo.Context.SaveChangesAsync();
                }



                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                _repo.Context.ChangeTracker.Clear();
            }
        }

        /// <summary>
        /// fetches previous record 
        /// updates dto current quantity and adds to context 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task AddWithUpdatedQuantityBasedOnPrevious(ModifyQuantityDto dto)
        {
            var previousInStore = await _repo.GetPreviousWithLockAsync(dto);

            if (dto.ModificationType != ModificationType.Buy)
                if (previousInStore == null)
                    throw new ArgumentException($"no previous entry found with less than {dto.EffectiveFrom} ");

            var qmStart =  _repo.AddQuantityMetric(dto.ProductId,
                                    CalculateQuantity(dto, 
                                    new ModifyQuantityDto() 
                                    {
                                                             Diff = previousInStore == null ? 0 : previousInStore.Diff,
                                                             Value = previousInStore == null ? 0 : previousInStore.Value, 
                                                             ModificationType = previousInStore == null ? 0 : previousInStore.ModificationType

                                    }
                                    ),
                                    dto.EffectiveFrom, dto.Diff, dto.ModificationType);

            EndLet(dto, qmStart);
        }



        /// <summary>
        /// adds the effective end equivalent record in quantity metrics 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="qmStart"></param>
        private void EndLet(ModifyQuantityDto dto, QuantityMetric qmStart)
        {
            if (dto.ModificationType != Contracts.ModificationType.Let)
                return;

            DateTime effectiveEndDate;

            if (_interval == ModifyQuantityInterval.Daily)
                effectiveEndDate = dto.EffectiveTo.AddDays(1); //todo do  i need this? do i need to increment by a day /hour /minute?

            else 
                throw new NotImplementedException();

            QuantityMetric qmEnd = new()
            {
                ProductId = dto.ProductId,
                Value = qmStart.Value + dto.Diff,
                Diff = dto.Diff,
                EffectiveDate = effectiveEndDate,
                ModificationType = ModificationType.EndLet
                
            };
            _repo.Context.QuantityMetrics.Add(qmEnd);
        }


        /// <summary>
        /// gets all post effecive date from records with lock  and updates quantity
        /// </summary>
        /// <param name="baseItem"></param>
        /// <returns></returns>
        private async Task ModifyQuantityPostEffectiveDate(ModifyQuantityDto baseItem)
        {
            var postEffectiveDateRows = await _repo.GetPostEffectiveDateRowsWithLockAsync(baseItem);

            foreach (var postItem in postEffectiveDateRows)
            {
                postItem.Value = CalculateQuantity(new ModifyQuantityDto()
                {
                    Diff = postItem.Diff,
                    Value = postItem.Value,
                    ModificationType = postItem.ModificationType
                }, baseItem);

                // point previous as current for next item to chain update values 
                baseItem = new ModifyQuantityDto() {  Diff = postItem.Diff, EffectiveFrom = postItem.EffectiveDate,
                                                      TransactionId = postItem.TransactionId, ProductId = postItem.ProductId,
                                                      ModificationType = postItem.ModificationType, Value = postItem.Value };
            }
        }

        /// <summary>
        /// given a previous Item and a current item updates currentItem Value based on previousItem 
        /// </summary>
        /// <param name="currentItem"></param>
        /// <param name="previousItem"></param>
        /// <returns></returns>
        private static decimal CalculateQuantity(ModifyQuantityDto currentItem,
                                                 ModifyQuantityDto previousItem) 
                                               
        {
            decimal previousItemValue = previousItem == null ? 0 : previousItem.Value;
           
            if (currentItem.ModificationType is Contracts.ModificationType.Let
                or Contracts.ModificationType.Sell
                )
                return previousItemValue - 1 * currentItem.Diff;
            else
                return previousItemValue + 1 * currentItem.Diff;
        }

     

    }
}
