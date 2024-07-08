using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Entities;
using Serilog;

namespace Inventory.Products.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ProductsDbContext _context;

        public InventoryRepository(ProductsDbContext context)
        { _context = context; }

        public bool CategoryFatherIdExists(Guid FatherId)
        {
            return _context.Categories
                   .Where(p => p.FatherId == FatherId)
                   .Count() > 0;
        }

        /// <summary>
        /// for testing 
        /// </summary>
        public void EmptyDB()
        {
            _context.Sources.RemoveRange(_context.Sources);
            _context.Inventories.RemoveRange(_context.Inventories);
            _context.Metrics.RemoveRange(_context.Metrics);
            _context.Categories.RemoveRange(_context.Categories);
            _context.ProductCategories.RemoveRange(_context.ProductCategories);
            _context.ProductMetrics.RemoveRange(_context.ProductMetrics);
            _context.Products.RemoveRange(_context.Products);
            _context.SaveChanges();
        }


        public bool InventoryIdExists(Guid Id)
        {
            return _context.Inventories.Where(p => p.Id == Id).Count() > 0;
        }

        public async Task<InventoryDto> AddInventoryAsync(InventoryDto c)
        {
            _context.Inventories.Add(new Entities.Inventory() {Id= c.Id, Description = c.Description });
            await _context.SaveChangesAsync();
            return new  InventoryDto(c.Id, c.Description);
        }

        public async Task<InventoryDto> EditInventoryAsync(InventoryDto c)
        {
            Entities.Inventory e = AddInventory(c);
            _context.Update(e);
            await _context.SaveChangesAsync();
            return new InventoryDto(c.Id, c.Description);
        }

        private static Entities.Inventory AddInventory(InventoryDto c)
        {
            return new Entities.Inventory() { Id = c.Id, Description = c.Description };
        }

        public async Task DeleteInventoryAsync(InventoryDto c)
        {
            List<Product> products = _context.Products.Where(p=>p.InventoryId == c.Id).ToList();

            foreach (Product pc in products)
                _context.Remove(pc);
            Entities.Inventory e = _context.Inventories.Where(p => p.Id == c.Id).Single();
            _context.Remove(e);

             await _context.SaveChangesAsync();           
        }

        public async Task<ProductDto> AddProductAsync(ProductDto c)
        {
            _context.Products.Add(new Entities.Product()
            { Description = c.Description, Id = c.Id, Code = c.Code, InventoryId = c.InventoryId });

            if (c.Metrics!= null)   
            foreach (var m in c.Metrics)
                DecideNewOrEdit(m);
            

            await _context.SaveChangesAsync();
            return new ProductDto(c.Id, c.Description,c.Code, c.InventoryId, c.Metrics);
        }

        public async Task<ProductDto> EditProductAsync(ProductDto c)
        {
            Product e = new Product()
            { Description = c.Description, Id = c.Id, Code = c.Code, InventoryId = c.InventoryId };
            _context.Update(e);

            foreach (var m in c.Metrics)
                DecideNewOrEdit(m);
            
            await _context.SaveChangesAsync();
            return new ProductDto(c.Id, c.Description,c.Code, c.InventoryId, c.Metrics);
        }

        public ProductDto GetProduct(Guid Id)
        {
            var entity = _context.Products.Where(p => p.Id == Id).Select(i => i).Single();
            return new ProductDto(entity.Id,
                                   entity.Description,
                                   entity.Code,
                                   entity.InventoryId,
                                   new List<ProductMetricDto>() // todo populate 
                                   );
        }

        public List<string> GetDistinctProductCodes(Guid InventoryId)
        {
            if (!_context.Products.Any())
                return new List<string>();
      
            return _context.Products
                 .Where(p=>p.InventoryId==InventoryId)
                 .Select(i => i.Code.Trim().ToUpper())
                 .GroupBy(p => p)
                 .Select(o => o.FirstOrDefault())
                 .Where(i=>!string.IsNullOrEmpty(i))
                 .ToList();
        }

        public List<string> GetDistinctMetricCodes()
        {
            if (!_context.Metrics.Any()) 
                return new List<string>();
        
            return _context.Metrics
                  .Select(i => i.Code.Trim().ToUpper())
                  .GroupBy(p => p)
                  .Select(o => o.FirstOrDefault())
                  .Where(i => !string.IsNullOrEmpty(i))
                  .ToList();
        }

        public async Task AddOrEditProductMetric(ProductMetricDto m)
        {
                // Log.Information("AddOrEditProductMetric:" + m.ToString());
                UpdateProductMetricCodes(m);
                DecideNewOrEdit(m);
                await _context.SaveChangesAsync();
        }

        public void UpdateProductMetricCodes(ProductMetricDto m)
        {
            string productCode = _context.Products.
                                 Where(p => p.Id == m.ProductId).
                                 Select(i => i.Code).Single();

            string metricCode = _context.Metrics.
                          Where(p => p.Id == m.MetricId).
                          Select(i => i.Code).Single();

            m.ProductCode = productCode;
            m.MetricCode = metricCode;

        }


        public  void DecideNewOrEdit(ProductMetricDto m)
        {
            var exists = _context.ProductMetrics.Where
                (
                    p => p.MetricId == m.MetricId
                         && p.ProductId == m.ProductId
                         && p.EffectiveDate == m.EffectiveDate
                ).ToList().Count() > 0; // needed tolist ? https://stackoverflow.com/questions/61052687/a-command-is-already-in-progress

            if (exists) 
            {
                ProductMetric pm = new();
                pm.ProductId = m.ProductId;
                pm.Value = m.Value; 
                pm.Currency = m.Currency;
                pm.ProductCode = m.ProductCode;
                pm.Currency = m.Currency;
                pm.EffectiveDate = m.EffectiveDate; 
                pm.MetricCode = m.MetricCode;   
                pm.MetricId = m.MetricId;   
                _context.Attach(pm);
                _context.Update(pm);
            }
            else
                _context.ProductMetrics.Add(CreateProductMetric(m));
        }

        private static ProductMetric CreateProductMetric(ProductMetricDto m)
        {
            return new ProductMetric()
            {
                MetricId = m.MetricId,
                EffectiveDate = m.EffectiveDate,
                ProductId = m.ProductId,
                Value = m.Value,
                Currency = m.Currency,
                ProductCode = m.ProductCode,
                MetricCode = m.MetricCode
            };
        }

        public async Task AddOrEditInventoryMetric(InventoryMetricDto m)
        {
            UpdateInventoryMetricCodes(m);
            DecideNewOrEdit(m);
            await _context.SaveChangesAsync();
        }

        public void UpdateInventoryMetricCodes(InventoryMetricDto m)
        {
            string InventoryCode = _context.Inventories.
                                 Where(p => p.Id == m.InventoryId).
                                 Select(i => i.Code).Single();

            string metricCode = _context.Metrics.
                          Where(p => p.Id == m.MetricId).
                          Select(i => i.Code).Single();

            m.InventoryCode = InventoryCode;
            m.MetricCode = metricCode;

        }


        public void DecideNewOrEdit(InventoryMetricDto m)
        {
            var metricExists = _context.InventoryMetrics
                               .Where(  p => p.MetricId == m.MetricId
                                     && p.InventoryId == m.InventoryId
                                     && p.EffectiveDate == m.EffectiveDate)
                               .Any();

            if (metricExists)
                _context.Update(m);
            else
                _context.InventoryMetrics.Add(CreateInventoryMetric(m));
        }

        private static InventoryMetric CreateInventoryMetric(InventoryMetricDto m)
        {
            return new InventoryMetric()
            {
                MetricId = m.MetricId,
                EffectiveDate = m.EffectiveDate,
                InventoryId = m.InventoryId,
                Value = m.Value,
                Currency = m.Currency,
                InventoryCode = m.InventoryCode,
                MetricCode = m.MetricCode
            };
        }

        public async Task DeleteProductAsync(ProductDto c)
        {
            List<ProductCategory> pcs = _context.ProductCategories
            .Where(p => p.ProductId == c.Id).ToList();
            
            foreach (ProductCategory pc in pcs)
                _context.Remove(pc);

            List<ProductMetric> pms = _context.ProductMetrics.
                Where(p => p.ProductId == c.Id).ToList();
           
            foreach (ProductMetric pm in pms)
                _context.Remove(pm);

            var p = _context.Products.
                         Where(p => p.Id == c.Id).
                         Single();
            _context.Remove(p);

            await _context.SaveChangesAsync();
        }

        public  bool ProductDescriptionOrCategoryIsUsed(ProductDto c)
        {
            return  _context.Products
                    .Where(  p => (p.Code == c.Code || p.Description == c.Description)
                                && p.Id != c.Id  )
                    .Count() > 0;
        }


        public bool CategoryIdExists(Guid Id)
        {
            return _context.Categories.Where(p => p.Id == Id).Count() > 0;
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
            _context.Update(e);
            await _context.SaveChangesAsync();
            return new CategoryDto(c.Id, c.Description, c.FatherId);
        }


        public async Task DeleteCategoryAsync(CategoryDto c)
        {
            List<ProductCategory> pcs = _context.ProductCategories
                                 .Where(p => p.CategoryId == c.Id)
                                 .ToList();
           
            foreach (ProductCategory pc in pcs)
                _context.Remove(pc);
            
            var e = _context.Categories
                                 .Where(p => p.Id == c.Id)
                                 .Single();
            _context.Remove(e);

            await _context.SaveChangesAsync();
        }


        public async Task<MetricDto> AddMetricAsync(MetricDto c)
        {
            _context.Metrics.Add(new Metric()
            {
                Code = c.Code,
                Id = c.Id,
                SourceId = c.SourceId,
                Description = c.Description,
            });
            await _context.SaveChangesAsync();
            return new MetricDto(c.Id, c.Description, c.Code, c.SourceId);
        }

        public async Task<MetricDto> EditMetricAsync(MetricDto c)
        {
            Metric e = new Metric()
            {
                Code = c.Code,
                Id = c.Id,
                SourceId = c.SourceId,
                Description = c.Description,
            };
            _context.Update(e);
            await _context.SaveChangesAsync();
            return new MetricDto(c.Id, c.Description, c.Code, c.SourceId);
        }


        public async Task DeleteMetricAsync(MetricDto c)
        {
            List<ProductMetric> pcs = _context.ProductMetrics
                                      .Where(p => p.MetricId == c.Id)
                                      .ToList();
            
            foreach (ProductMetric pc in pcs)
                _context.Remove(pc);

            Metric e = _context.Metrics
                       .Where(p => p.Id == c.Id)
                       .Single();
            
            _context.Remove(e);
            await _context.SaveChangesAsync();
        }


        public MetricDto GetMetric(Guid metricId)
        {
            var entity = _context.Metrics
                         .Where(p =>  p.Id == metricId)
                         .Select(i => i)
                         .Single();

            return new MetricDto(entity.Id, entity.Description, entity.Code, entity.SourceId);
        }


        public ProductMetricDto GetProductMetric(Guid productId, Guid metricId)
        {
            var entity = _context.ProductMetrics
                         .Where(p => p.ProductId == productId && p.MetricId == metricId)
                         .Select(i => i)
                         .Single();

            return new ProductMetricDto(entity.ProductId,
                                        entity.MetricId,
                                        entity.Value,
                                        entity.EffectiveDate,
                                        entity.Currency ,
                                        entity.ProductCode,
                                        entity.MetricCode);
        }

        public async  Task<SourceDto> AddSourceAsync(SourceDto dto )
        {
            Entities.Source e = new Entities.Source() 
                                { Id = dto.Id, Description = dto.Description  };
            _context.Sources.Add(e);
            await _context.SaveChangesAsync();
            return new SourceDto(dto.Id, dto.Description);
        }


        /// <summary>
        /// returns the latest by effective date product metric row 
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <param name="MetricCode"></param>
        /// <returns></returns>
        public  ProductMetricDto GetProductMetric(string ProductCode, string MetricCode)
        {
                ProductCode = ProductCode.ToUpper().Trim();
                MetricCode = MetricCode.ToUpper().Trim();

                return _context
                       .ProductMetrics
                       .Where(i => i.ProductCode == ProductCode  && i.MetricCode == MetricCode)
                       .OrderByDescending(i => i.EffectiveDate)
                       .Select(i => new ProductMetricDto(i.ProductId,
                                                         i.MetricId,
                                                         i.Value,
                                                         i.EffectiveDate,
                                                         i.Currency,
                                                         i.ProductCode,
                                                         i.MetricCode))
                       .First();

         

        }

        
    }
}
