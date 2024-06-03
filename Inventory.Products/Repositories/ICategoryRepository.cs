using Category.Products.Endpoints;
using Inventory.Products.Dto;

namespace Inventory.Products.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryDto> AddCategoryAsync(CategoryDto dto);
        bool FatherIdExists(Guid FatherId);
    }
}