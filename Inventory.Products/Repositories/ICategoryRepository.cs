using Inventory.Products.Dto;

namespace Inventory.Products.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryDto> AddCategoryAsync(CategoryDto dto);
        bool FatherIdExists(Guid FatherId);
        Task<CategoryDto> EditCategoryAsync(CategoryDto c);
        Task DeleteCategoryAsync(CategoryDto c);

    }
}