using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Repositories
{
    public  interface IModifyQuantityRepository
    {
        public Task AddBasedOnPrevious(ModifyQuantityDto dto);
        public Task ModifyQuantityMetrics(List<ModifyQuantityDto> inboundQuantities);
    }
}
