using Inventory.Products.Dto;


namespace Inventory.Products.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Products.ProductsDbContext _context;
        public CategoryRepository(ProductsDbContext context) { _context = context; }

        public bool FatherIdExists(Guid FatherId)
        {
            return _context.Categories.Where(p => p.FatherId == FatherId).Count() > 0;
        }

        public async Task<CategoryDto> AddCategoryAsync(CategoryDto c)
        {
            _context.Categories.Add(new Entities.Category() {FatherId= c.FatherId,Name = c.Description  });
            await _context.SaveChangesAsync();
            return new  CategoryDto(c.Id, c.Description, c.FatherId);
        }


    }
}
