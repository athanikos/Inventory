using Inventory.Products.Contracts.Dto;
using Inventory.Products.Repositories;

namespace Inventory.Products.Services
{
    public class ModifyQuantityService : IModifyQuantityService
    {
        private readonly IModifyQuantityRepository _repo;

        public ModifyQuantityService(IModifyQuantityRepository repo)
        {
            _repo = repo;
        }

        public async Task ModifyQuantityMetrics(List<ModifyQuantityDto> inboundQuantities)
        {
            await _repo.ModifyQuantityMetrics(inboundQuantities);
        }
    }
}
