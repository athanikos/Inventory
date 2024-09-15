using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Entities;
using Inventory.Products.Repositories;

namespace Inventory.Products.Services
{
    /// <summary>
    ///  Implements quantity increment / decrement with locks in quantity metric entity  
    ///  Validates inbound intervals against store entries
    ///  Validates inbound entries aginst each other 
    /// </summary>
    public class ModifyQuantityService : IModifyQuantityService
    {
        private readonly IModifyQuantityRepository _repo;
        private ModifyQuantityInterval _interval = ModifyQuantityInterval.Daily;

        public enum ModifyQuantityInterval
        {
            Daily = 0 ,
            Hourly = 1 ,    
            Minutely = 2 ,  
        }

        public ModifyQuantityService(IModifyQuantityRepository repo)
        {
            _repo = repo;
        }

        private  static void  Validate(List<ModifyQuantityDto> inboundQuantities)
        {
            if  (inboundQuantities.Where(o=>o.Diff < 0).Any())
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
                                               where ModificationTypeHelper.IsBuyOrSell( i.ModificationType)
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
                        IsCancelled = previousItem.IsCancelled
                    };

                    await ModifyQuantityPostEffectiveDate(qm);


                }

                 
                await _repo.Context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
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
            inboundQuantities = inboundQuantities.OrderBy(o => o.EffectiveFrom).ToList();
            await using var transaction = await _repo.Context.Database.BeginTransactionAsync();
            try
            {
              
                foreach (var item in inboundQuantities)
                {
                    await AddWithUpdatedQuantityBasedOnPrevious(item);
                    await ModifyQuantityPostEffectiveDate(item);
                    await _repo.Context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }
            catch (Exception ex)
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

            if (dto.ModificationType != Contracts.ModificationType.Buy)
                if (previousInStore == null)
                    throw new ArgumentException($"no previous entry found with less than {dto.EffectiveFrom} ");

            var qmStart =  _repo.AddQuantityMetric(dto.ProductId,
                                    CalculateQuantity(dto, previousInStore),
                                    dto.EffectiveFrom);

            Unlet(dto, qmStart);`
        }



        /// <summary>
        /// adds the effective end equivalent record in quantity metrics 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="qmStart"></param>
        private void Unlet(ModifyQuantityDto dto, QuantityMetric qmStart)
        {
            if (dto.ModificationType != Contracts.ModificationType.Let)
                return;

            DateTime effectiveEndDate;

            if (_interval == ModifyQuantityInterval.Daily)
                effectiveEndDate = dto.EffectiveTo.AddDays(1);
            else 
                throw new NotImplementedException();

            QuantityMetric qmEnd = new QuantityMetric()
            {
                ProductId = dto.ProductId,
                Value = qmStart.Value + dto.Diff,
                EffectiveDate = effectiveEndDate
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
                 postItem.Value = CalculateQuantity(baseItem, postItem);
        }

        /// <summary>
        /// given a previous Item and a current item updates currentItem Value based on previousItem 
        /// when modification type is buy or sell 
        /// </summary>
        /// <param name="currentItem"></param>
        /// <param name="previousItem"></param>
        /// <returns></returns>
        private static decimal CalculateQuantity(ModifyQuantityDto currentItem, 
                                                 QuantityMetric previousItem) 
                                               
        {
            decimal newValue = 0;
            decimal itemValue = previousItem ==null ? 0 : previousItem.Value;

            if (currentItem.ModificationType == Contracts.ModificationType.Buy)
                newValue = itemValue + 1 * currentItem.Diff;
            else if (ModificationTypeHelper.IsBuyOrSell(currentItem.ModificationType))
                newValue = itemValue - 1 * currentItem.Diff;
            return newValue;
        }

     

    }
}
