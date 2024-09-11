using Inventory.Products.Contracts.Dto;
using Inventory.Products.Endpoints;
using Inventory.Products.Entities;
using Inventory.Products.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Products.Services
{
    /// <summary>
    ///  Implements quantity increment / decrement with locks in quantity metric entity  
    ///  Validates inbound intervals against store entries
    ///  Validates inbound entries aginst each other 
    ///  
    /// </summary>
    public class ModifyQuantityService : IModifyQuantityService
    {
        private readonly IModifyQuantityRepository _repo;

        public ModifyQuantityService(IModifyQuantityRepository repo)
        {
            _repo = repo;
        }

        public async Task ModifyQuantityMetricsAsync(List<ModifyQuantityDto> inboundQuantities)
        {
            await using var transaction = await _repo.Context.Database.BeginTransactionAsync();
            try
            {
                InboundEntitiesAreOverlapping(inboundQuantities);

                foreach (var item in inboundQuantities)
                {
                    await AddBasedOnPrevious(item);
                    await ModifyQuantityPostEffectiveDate(item);
                    await _repo.HasOverlappingRecordsWithLockAsync(item);
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


        private async Task  AddBasedOnPrevious(ModifyQuantityDto dto)
        {
            var previousInTimeEntry = await _repo.GetPreviousWithLockAsync(dto);

            if (dto.ModificationType != Contracts.ModificationType.Buy)
                if (previousInTimeEntry == null)
                    throw new ArgumentException($"no previous entry found with less than {dto.EffectiveFrom} ");

            _repo.AddQuantityMetric(dto.ProductId, 
                                    CalculateQuantity(dto, previousInTimeEntry), 
                                    dto.EffectiveFrom);
        }

      

        private void InboundEntitiesAreOverlapping(List<ModifyQuantityDto> inboundQuantities)
        {
            var indexedList = inboundQuantities
                       .Select((dto, index) => new { Index = index, Dto = dto });


            if ((
                        from item in indexedList
                        join otherItem in indexedList
                        on item.Dto.ProductId equals otherItem.Dto.ProductId
                        select new { item, otherItem }
                    ).Where
                    (
                        i => i.item.Index != i.otherItem.Index &&
                            (
                                    i.item.Dto.EffectiveFrom >= i.otherItem.Dto.EffectiveFrom
                                    &&
                                    i.item.Dto.EffectiveFrom <= i.otherItem.Dto.EffectiveTo
                            )
                    ).Any())
                throw new ArgumentException();



        }

        public async Task ModifyQuantityPostEffectiveDate(ModifyQuantityDto dto)
        {
            if (dto.ModificationType == Contracts.ModificationType.Let)
                return;

            var postEffectiveDateRows = await _repo.GetPostEffectiveDateRowsWithLockAsync(dto);

            foreach (var item in postEffectiveDateRows)
                 item.Value = CalculateQuantity(dto, item);
            

        }

        private static decimal CalculateQuantity(ModifyQuantityDto cureentItem, QuantityMetric previousItem)
        {
            decimal newValue = 0;
            decimal itemValue = previousItem ==null ? 0 : previousItem.Value;

            if (cureentItem.ModificationType == Contracts.ModificationType.Buy)
                newValue = itemValue + cureentItem.Diff;
            else if (cureentItem.ModificationType == Contracts.ModificationType.Sell
                       || cureentItem.ModificationType == Contracts.ModificationType.Let
                    )
                newValue = itemValue - cureentItem.Diff;
            return newValue;
        }
    }
}
