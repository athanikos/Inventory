using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Services
{
    public interface IModifyQuantityService
    {
        Task ModifyQuantityMetrics(List<ModifyQuantityDto> inboundQuantities);
    }
}