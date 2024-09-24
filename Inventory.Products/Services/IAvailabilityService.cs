using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Services
{
    public  interface  IAvailabilityService
    { 
        /// <summary>
        /// across all products (rooms) within the inventoryId 
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        List<ModifyQuantityDto> GetAvailabilityPerInventory(Guid inventoryId,  DateTime from, DateTime to);

        
        /// <summary>
        /// for one product (room) 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        List<ModifyQuantityDto> GetAvailabilityPerProduct(Guid productId, DateTime from, DateTime to);

    }
}
