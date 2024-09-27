using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Products.Repositories
{
    public class PostgresInventoryRepository : IInventoryRepository
    {
        private readonly ProductsDbContext _context;

        public PostgresInventoryRepository(ProductsDbContext context)
        { _context = context; }

        public Task<bool> CategoryFatherIdExistsAsync(Guid FatherId)
        {
            return _context.Categories
                   .Where(p => p.FatherId == FatherId)
                   .AnyAsync();
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
            _context.QuantityMetrics.RemoveRange(_context.QuantityMetrics);
            _context.Products.RemoveRange(_context.Products);
            _context.SaveChanges();
        }

        #region Inventory
        public bool InventoryIdExists(Guid id)
        {
            return _context.Inventories.Any(p => p.Id == id);
        }

        public async Task<InventoryDto> GetInventoryAsync(Guid inventoryId)
        {
            return await _context
                .Inventories.Where(o => o.Id == inventoryId)
                .Select(i=>new InventoryDto(i.Id,i.Description))
                .SingleAsync();
        }
        
        public async Task<InventoryDto> AddInventoryAsync(InventoryDto c)
        {
            var e = new Entities.Inventory() { Id = c.Id, Description = c.Description };
            _context.Inventories.Add(e);
            await _context.SaveChangesAsync();
            return await GetInventoryAsync(e.Id);
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
            List<Product> products = [.. _context.Products.Where(p=>p.InventoryId == c.Id)];

            foreach (Product pc in products)
                _context.Remove(pc);
            Entities.Inventory e = _context.Inventories.Where(p => p.Id == c.Id).Single();
            _context.Remove(e);

             await _context.SaveChangesAsync();           
        }
        #endregion

        public async Task<ProductDto> AddProductAsync(ProductDto c)
        {
            var p = new Product() { Description = c.Description, 
                Id = c.Id, Code = c.Code, InventoryId = c.InventoryId };
            
            _context.Products.Add(p);

            foreach (var m in c.Metrics)
                DecideNewOrEdit(m);


            await _context.SaveChangesAsync();

            return await GetProductAsync(p.Id);
        }

        public async Task<ProductDto> EditProductAsync(ProductDto c)
        {
            Product p = new Product()
            { Description = c.Description, Id = c.Id, Code = c.Code, InventoryId = c.InventoryId };
            _context.Update(p);

            foreach (var m in c.Metrics)
                DecideNewOrEdit(m);
            
            await _context.SaveChangesAsync();
            return await GetProductAsync(p.Id);
        }

        private async  Task<ProductDto> GetProductAsync(Guid id)
        {
            var entity = await  _context.Products.Where(p => p.Id == id).Select(i => i).SingleAsync();
            return new ProductDto(entity.Id,
                                   entity.Description,
                                   entity.Code,
                                   entity.InventoryId,
                                   [] // todo populate 
                                   );
        }

        public List<string> GetDistinctProductCodes(Guid inventoryId)
        {
            if (!_context.Products.Any())
                return [];

            return [.. _context.Products
                    .Where(r => !String.IsNullOrEmpty(r.Code))
                    .Select(t => t.Code.ToUpper())
                    .Distinct()];
        }

        public  List<string> GetDistinctMetricCodes()
        {
            if (!_context.Metrics.Any()) 
                return [];
        
            return [.. _context.Metrics
                         .Where(r => !String.IsNullOrEmpty(r.Code))
                         .Select(t=> t.Code.ToUpper())
                         .Distinct()];
        }
           
        public async Task AddOrEditProductMetricAsync(ProductMetricDto m)
        {
                UpdateProductMetricCodes(m);
                DecideNewOrEdit(m);
                await _context.SaveChangesAsync();
        }

        public void AddOrEditProductMetric(ProductMetricDto m)
        {
            UpdateProductMetricCodes(m);
            DecideNewOrEdit(m);
        }

        public async Task<ProductMetricDto> GetProductMetricAsync(string productCode, string metricCode, DateTime effectivedate )
        {
           return  await _context.ProductMetrics
               .Where(p=>p.MetricCode== metricCode && p.ProductCode == productCode
                                        && p.EffectiveDate == effectivedate)
               .Select ( i=> new ProductMetricDto(i.ProductId,i.MetricId, i.Value, 
                   i.EffectiveDate, i.ProductCode, i.MetricCode,i.UnitOfMeasurementId))
               .SingleAsync();
        }

        public async Task<ProductMetricDto> GetProductMetricAsync(Guid productId, DateTime effectivedate)
        {
            return await _context.ProductMetrics
                                  .Where(p => p.ProductId == productId
                                         && p.EffectiveDate == effectivedate)
                                  .Select(i => new ProductMetricDto(i.ProductId, i.MetricId, i.Value, i.EffectiveDate,
                                                                i.ProductCode, i.MetricCode, i.UnitOfMeasurementId))
                                  .SingleAsync();
        }

        public void UpdateProductMetricCodes(ProductMetricDto m)
        {
            string productCode, metricCode;
            UpdateMetricCodes(m.ProductId, m.MetricId, out productCode, out metricCode);

            m.ProductCode = productCode;
            m.MetricCode = metricCode;

        }
                
        private void UpdateMetricCodes(Guid productId, Guid metricId, out string productCode, out string metricCode)
        {
            productCode = _context.Products.
                                 Where(p => p.Id == productId).
                                 Select(i => i.Code).Single();

            metricCode = _context.Metrics.
                          Where(p => p.Id == metricId).
                          Select(i => i.Code).Single();
        }

        public void DecideNewOrEdit(ProductMetricDto m)
        {
            var pm = _context.ProductMetrics.FirstOrDefault(p => p.MetricId == m.MetricId
                                                                 && p.ProductId == m.ProductId
                                                                 && p.EffectiveDate == m.EffectiveDate); // needed tolist ? https://stackoverflow.com/questions/61052687/a-command-is-already-in-progress

            if (pm != null)
            {
                pm.ProductId = m.ProductId;
                pm.Value = m.Value;
                pm.ProductCode = m.ProductCode;
                pm.UnitOfMeasurementId = m.UnitOfMeasurementId;
                pm.EffectiveDate = m.EffectiveDate;
                pm.MetricCode = m.MetricCode;
                pm.MetricId = m.MetricId;
                _context.Update(pm);
            }
            else
                _context.ProductMetrics.Add(ProductMetric.CreateProductMetric(m));
        }

        public void DecideNewOrEdit(QuantityMetricDto m)
        {
            var qm = _context.QuantityMetrics.FirstOrDefault(p => p.ProductId == m.ProductId
                                                                  && p.EffectiveDate == m.EffectiveDate); // needed tolist ? https://stackoverflow.com/questions/61052687/a-command-is-already-in-progress

            if (qm!=null) 
            {
                qm.ProductId = m.ProductId;
                qm.Value = m.Value; 
                qm.EffectiveDate = m.EffectiveDate; 
                _context.Update(qm);
            }
            else
                _context.QuantityMetrics.Add(QuantityMetric.CreateQuantityMetric(m));
        }

        public async Task AddOrEditInventoryMetric(InventoryMetricDto m)
        {
            UpdateInventoryMetricCodes(m);
            DecideNewOrEdit(m);
            await _context.SaveChangesAsync();
        }

        public void UpdateInventoryMetricCodes(InventoryMetricDto m)
        {
            string inventoryCode = _context.Inventories.
                                 Where(p => p.Id == m.InventoryId).
                                 Select(i => i.Code).Single();

            string metricCode = _context.Metrics.
                          Where(p => p.Id == m.MetricId).
                          Select(i => i.Code).Single();

            m.InventoryCode = inventoryCode;
            m.MetricCode = metricCode;

        }

        public void DecideNewOrEdit(InventoryMetricDto m)
        {
            var metricExists = _context.InventoryMetrics
                .Any(p => p.MetricId == m.MetricId
                          && p.InventoryId == m.InventoryId
                          && p.EffectiveDate == m.EffectiveDate);

            if (metricExists)
                _context.Update(m);
            else
                _context.InventoryMetrics.Add(InventoryMetric.CreateInventoryMetric(m));
        }

        public async Task DeleteProductAsync(ProductDto c)
        {
            List<ProductCategory> pcs = [.. _context.ProductCategories.
                                            Where(p => p.ProductId == c.Id)];
            
            foreach (ProductCategory pc in pcs)
                _context.Remove(pc);

            List<ProductMetric> pms = [.. _context.ProductMetrics.
                                           Where(p => p.ProductId == c.Id)];
           
            foreach (ProductMetric pm in pms)
                _context.Remove(pm);

            var p = _context.Products.
                Single(p => p.Id == c.Id);
            _context.Remove(p);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ProductDescriptionOrCategoryIsUsedAsync(ProductDto c)
        {
            return await   _context.Products
                           .Where(  p => (p.Code == c.Code || p.Description == c.Description)
                            && p.Id != c.Id  )
                           .AnyAsync();    
        }

        public async  Task<bool> CategoryIdExistsAsync(Guid Id)
        {
            return await _context.Categories
                         .Where(p => p.Id == Id).AnyAsync(); 
        }

        public async Task<CategoryDto> AddCategoryAsync(CategoryDto c)
        {
            _context.Categories.Add(new Category() { FatherId = c.FatherId, Name = c.Description });
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
            List<ProductCategory> pcs = [.. _context.ProductCategories.Where(p => p.CategoryId == c.Id)];
           
            foreach (ProductCategory pc in pcs)
                _context.Remove(pc);
            
            var e = _context.Categories
                .Single(p => p.Id == c.Id);
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
            List<ProductMetric> pcs = [.. _context.ProductMetrics.Where(p => p.MetricId == c.Id)];
            
            foreach (ProductMetric pc in pcs)
                _context.Remove(pc);

            Metric e = _context.Metrics
                .Single(p => p.Id == c.Id);
            
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
            /// <param name="productCode"></param>
            /// <param name="MetricCode"></param>
            /// <returns></returns>
        public async Task<ProductMetricDto> GetProductMetricAsync(string productCode, string MetricCode)
        {
            productCode = productCode.ToUpper().Trim();
            MetricCode = MetricCode.ToUpper().Trim();


            if (MetricCode == Constants.Quantitycode)
            {

                // todo cache reuse allcodes field add to repo and do lazy loading ? or refresh evry 
                var productInfo  =await  _context
                       .Products
                       .Where(i => i.Code == productCode)    
                       .Select(i => new { i.Id,  i.Code })
                       .SingleAsync();

                var metricInfo = await _context
                  .Metrics
                  .Where(i => i.Code == MetricCode)
                  .Select(i => new { i.Id, i.Code })
                  .SingleAsync();


                var item =  await _context
                .QuantityMetrics
                .Where(u => u.ProductId == productInfo.Id)
                .OrderByDescending(i => i.EffectiveDate)
                .Select(i => new ProductMetricDto(i.ProductId,
                                                  metricInfo.Id,
                                                  i.Value,
                                                  i.EffectiveDate,
                                                  productInfo.Code,
                                                  Constants.Quantitycode,
                                                  Constants.EmptyUnityOfMeasurementId
                                                  )).FirstOrDefaultAsync();


                if (item != null)
                    return new ProductMetricDto(item.ProductId,item.MetricId, item.Value,item.EffectiveDate,item.Currency,item.ProductCode
                        
                        ,item.UnitOfMeasurementId);

                return new ProductMetricDto(Guid.NewGuid());
            }
            else
            {

                       return await _context
                       .ProductMetrics
                       .Where(i => i.ProductCode == productCode && i.MetricCode == MetricCode)
                       .OrderByDescending(i => i.EffectiveDate)
                       .Select(i => new ProductMetricDto(i.ProductId,
                                                         i.MetricId,
                                                         i.Value,
                                                         i.EffectiveDate,
                                                    
                                                         i.ProductCode,
                                                         i.MetricCode,
                                                         i.UnitOfMeasurementId))
                       .FirstAsync();

            }

        }
        /// <summary>
        /// adds to quantity metric and product metric table 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<QuantityMetricDto> AddQuantityMetricAsync(QuantityMetricDto dto)
        {
            QuantityMetric qm = new QuantityMetric();
            qm.ProductId = dto.ProductId;
            qm.Value = dto.Value;
            qm.Diff = dto.Diff;
            qm.IsCancelled = dto.IsCancelled;
            qm.EffectiveDate = dto.EffectiveDate;
            _context.Add(qm);
            await _context.SaveChangesAsync();
            return await  GetQuantityMetricAsync(qm.ProductId, qm.EffectiveDate);
        }


      



        public async Task<QuantityMetricDto> EditQuantityMetricAsync(QuantityMetricDto dto)
        {

            QuantityMetric qm = new QuantityMetric();
            qm.ProductId = dto.ProductId;
            qm.Value = dto.Value;
            qm.Diff = dto.Diff;
            qm.IsCancelled = dto.IsCancelled;
            qm.EffectiveDate = dto.EffectiveDate;
            _context.Update(qm);
            await _context.SaveChangesAsync();
            return await GetQuantityMetricAsync(qm.ProductId, qm.EffectiveDate);
        }

        public void AddQuantityMetric(QuantityMetricDto dto)
        {
            QuantityMetric qm = new QuantityMetric();
            qm.ProductId = dto.ProductId;
            qm.Value = dto.Value;
            qm.EffectiveDate = dto.EffectiveDate;
            qm.Diff = dto.Diff;
            qm.IsCancelled = dto.IsCancelled;   
            qm.TransactionId = dto.TransactionId;
           
            _context.Add(qm);
        }

        public async Task<QuantityMetricDto> GetQuantityMetricAsync(Guid id, DateTime EffectiveDate)
        {
            return  await _context.QuantityMetrics.
                                   Where(i => i.ProductId == id && i.EffectiveDate == EffectiveDate).
                                   Select(i =>new QuantityMetricDto(i.ProductId,i.Value,i.EffectiveDate,i.TransactionId, i.Diff,i.IsCancelled)).
                                   SingleAsync();

        }

        public async Task<List<QuantityMetricDto>> GetQuantityMetricsAsync(Guid TransactionId)
        {
            return await _context.QuantityMetrics.
                                   Where(i => i.TransactionId == TransactionId).
                                   Select(i => new QuantityMetricDto(i.ProductId, i.Value, i.EffectiveDate, i.TransactionId, i.Diff, i.IsCancelled)).
                                   ToListAsync();

        }



        public async Task<List<QuantityMetricDto>> GetQuantityMetricsAsync()
        {
            return await _context.QuantityMetrics.
                                    Select(i => new QuantityMetricDto(i.ProductId, i.Value, i.EffectiveDate, i.TransactionId, i.Diff, i.IsCancelled)).
                                   ToListAsync();

        }



        public async Task<List<QuantityMetricDto>>
          CancellQuantityMetricsAsync(Guid transactionId)
        {
            var qms = await GetQuantityMetricsAsync(transactionId);
            foreach (var item in qms)
                item.IsCancelled = true;

            await _context.SaveChangesAsync();

            return await GetQuantityMetricsAsync(transactionId);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
