using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Services
{
    public  class AvailabilityService  : IAvailabilityService
    {
        public List<ModifyQuantityDto> GetAvailabilityPerInventory(Guid inventoryId, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public List<ModifyQuantityDto> GetAvailabilityPerProduct(Guid productId, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}
