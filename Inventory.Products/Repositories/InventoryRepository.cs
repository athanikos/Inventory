using Inventory.Products.Dto;


namespace Inventory.Products.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly Products.ProductsDbContext _context;
        
        public InventoryRepository(ProductsDbContext context)
        { _context = context; }

        public bool InventoryFatherIdExists(Guid FatherId)
        {
            return _context.Categories.Where(p => p.FatherId == FatherId).Count() > 0;
        }

        public async Task<InventoryDto> AddInventoryAsync(InventoryDto c)
        {
            _context.Inventories.Add(new Entities.Inventory() {Id= c.Id, Description = c.Description });
            await _context.SaveChangesAsync();
            return new  InventoryDto(c.Id, c.Description);
        }

        public async Task<InventoryDto> EditInventoryAsync(InventoryDto c)
        {
            Entities.Inventory e = new Entities.Inventory() { Id = c.Id, Description = c.Description };
            _context.Update(e);
             await _context.SaveChangesAsync();
            return new InventoryDto(c.Id, c.Description);
        }


        public async Task DeleteInventoryAsync(InventoryDto c)
        {
            List<Entities.Product> products = _context.Products.Where(p=>p.InventoryId == c.Id).ToList();
            foreach (Entities.Product pc in products)
                _context.Remove(pc);
            Entities.Inventory e = _context.Inventories.Where(p => p.Id == c.Id).Single();
            _context.Remove(e);

             await _context.SaveChangesAsync();           
        }

        public async Task<ProductDto> AddProductAsync(ProductDto c)
        {
            _context.Products.Add(new Entities.Product()
            { Description = c.Description, Id = c.Id, InventoryId = c.InventoryId });
            await _context.SaveChangesAsync();
            return new ProductDto(c.Id, c.Description, c.InventoryId);
        }

        public async Task<ProductDto> EditProductAsync(ProductDto c)
        {
            Entities.Product e = new Entities.Product()
            { Description = c.Description, Id = c.Id, InventoryId = c.InventoryId };
            _context.Update(e);
            await _context.SaveChangesAsync();
            return new ProductDto(c.Id, c.Description, c.InventoryId);
        }


        public async Task DeleteProductAsync(ProductDto c)
        {
            List<Entities.ProductCategory> pcs = _context.ProductCategories.Where(p => p.ProductId == c.Id).ToList();
            foreach (Entities.ProductCategory pc in pcs)
                _context.Remove(pc);
            Entities.Product e = _context.Products.Where(p => p.Id == c.Id).Single();
            _context.Remove(e);

            await _context.SaveChangesAsync();
        }

        public bool CategoryFatherIdExists(Guid FatherId)
        {
            return _context.Categories.Where(p => p.FatherId == FatherId).Count() > 0;
        }

        public async Task<CategoryDto> AddCategoryAsync(CategoryDto c)
        {
            _context.Categories.Add(new Entities.Category() { FatherId = c.FatherId, Name = c.Description });
            await _context.SaveChangesAsync();
            return new CategoryDto(c.Id, c.Description, c.FatherId);
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
            List<Entities.ProductCategory> pcs = _context.ProductCategories.Where(p => p.CategoryId == c.Id).ToList();
            foreach (Entities.ProductCategory pc in pcs)
                _context.Remove(pc);
            Entities.Category e = _context.Categories.Where(p => p.Id == c.Id).Single();
            _context.Remove(e);

            await _context.SaveChangesAsync();
        }


        public async Task<MetricDto> AddMetricAsync(MetricDto c)
        {
            _context.Metrics.Add(new Entities.Metric()
            {
                Code = c.Code,
                Id = c.Id,
                SourceId = c.SourceId,
                Value = c.Value,
                Description = c.Description,
                EffectiveDate = c.EffectiveDate
            });
            await _context.SaveChangesAsync();
            return new MetricDto(c.Id, c.Description, c.Value, c.EffectiveDate, c.Code, c.SourceId);
        }

        public async Task<MetricDto> EditMetricAsync(MetricDto c)
        {
            Entities.Metric e = new Entities.Metric()
            {
                Code = c.Code,
                Id = c.Id,
                SourceId = c.SourceId,
                Value = c.Value,
                Description = c.Description,
                EffectiveDate = c.EffectiveDate
            };
            _context.Update(e);
            await _context.SaveChangesAsync();
            return new MetricDto(c.Id, c.Description, c.Value, c.EffectiveDate, c.Code, c.SourceId);
        }


        public async Task DeleteMetricAsync(MetricDto c)
        {
            List<Entities.ProductMetric> pcs = _context.ProductMetrics.Where(p => p.MetricId == c.Id).ToList();
            foreach (Entities.ProductMetric pc in pcs)
                _context.Remove(pc);
            Entities.Metric e = _context.Metrics.Where(p => p.Id == c.Id).Single();
            _context.Remove(e);

            await _context.SaveChangesAsync();
        }

    }
}
