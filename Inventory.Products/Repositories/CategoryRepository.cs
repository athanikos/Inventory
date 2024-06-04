using Inventory.Products.Dto;
using Inventory.Products.Entities;


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

        public async Task<CategoryDto> EditCategoryAsync(CategoryDto c)
        {
            Entities.Category e = new Entities.Category() { FatherId = c.FatherId, Name = c.Description, Id = c.Id };
            _context.Attach(e);
            _context.Entry(e).Property(p => p.Name).IsModified = true;
            await _context.SaveChangesAsync();
            return new CategoryDto(c.Id, c.Description, c.FatherId);
        }


        public async Task DeleteCategoryAsync(CategoryDto c)
        {
            Entities.Category e = _context.Categories.Where(p => p.Id == c.Id).Single();
            _context.Remove(e);
             await _context.SaveChangesAsync();           
        }

    }
}
