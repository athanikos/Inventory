using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Services
{
    public interface IModifyQuantityService
    {
        Task ModifyQuantityMetricsAsync(List<ModifyQuantityDto> inboundQuantities);

        Task CancelQuantityMetricsAsync(List<ModifyQuantityDto> inboundQuantities);
    }
}