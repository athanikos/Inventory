using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Entities;
using Inventory.Products.Handlers;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace Inventory.Products.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
    
   


        
        private readonly ProductsDbContext _context;

        public InventoryRepository(ProductsDbContext context)
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
        #endregion 


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

        public async  Task<ProductDto> GetProductAsync(Guid Id)
        {
            var entity = await  _context.Products.Where(p => p.Id == Id).Select(i => i).SingleAsync();
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

            return  _context.Products
                    .Where(r => !String.IsNullOrEmpty(r.Code))
                    .Select(t => t.Code.ToUpper())
                    .Distinct()
                    .ToList();
        }

        public  List<string> GetDistinctMetricCodes()
        {
            if (!_context.Metrics.Any()) 
                return new List<string>();
        
            return  _context.Metrics
                         .Where(r => !String.IsNullOrEmpty(r.Code))
                         .Select(t=> t.Code.ToUpper())
                         .Distinct()    
                         .ToList();
        }
           
        public async Task AddOrEditProductMetricAsync(ProductMetricDto m)
        {
                UpdateProductMetricCodes(m);
                DecideNewOrEdit(m);
                await _context.SaveChangesAsync();
        }

        public async Task<ProductMetricDto> GetProductMetricAsync(string productCode, string metricCode, DateTime effectivedate )
        {
           return  await _context.ProductMetrics
                                 .Where(p=>p.MetricCode== metricCode && p.ProductCode == productCode
                                        && p.EffectiveDate == effectivedate)
                                 .Select ( i=> new ProductMetricDto(i.ProductId,i.MetricId, i.Value, i.EffectiveDate, 
                                                                 i.Currency, i.ProductCode, i.MetricCode))
                                 .SingleAsync();
        }

        public async Task<ProductMetricDto> GetProductMetricAsync(Guid productId, DateTime effectivedate)
        {
            return await _context.ProductMetrics
                                  .Where(p => p.ProductId == productId
                                         && p.EffectiveDate == effectivedate)
                                  .Select(i => new ProductMetricDto(i.ProductId, i.MetricId, i.Value, i.EffectiveDate,
                                                                  i.Currency, i.ProductCode, i.MetricCode))
                                  .SingleAsync();
        }


        public void UpdateProductMetricCodes(ProductMetricDto m)
        {
            string productCode, metricCode;
            UpdateMetricCodes(m.ProductId, m.MetricId, out productCode, out metricCode);

            m.ProductCode = productCode;
            m.MetricCode = metricCode;

        }
                
        private void UpdateMetricCodes(Guid ProductId, Guid MetricId, out string productCode, out string metricCode)
        {
            productCode = _context.Products.
                                 Where(p => p.Id == ProductId).
                                 Select(i => i.Code).Single();
            metricCode = _context.Metrics.
                          Where(p => p.Id == MetricId).
                          Select(i => i.Code).Single();
        }

        public void DecideNewOrEdit(ProductMetricDto m)
        {
            var pm = _context.ProductMetrics.Where
                (
                    p => p.MetricId == m.MetricId
                         && p.ProductId == m.ProductId
                         && p.EffectiveDate == m.EffectiveDate
                ).FirstOrDefault(); // needed tolist ? https://stackoverflow.com/questions/61052687/a-command-is-already-in-progress

            if (pm != null)
            {
                pm.ProductId = m.ProductId;
                pm.Value = m.Value;
                pm.Currency = m.Currency;
                pm.ProductCode = m.ProductCode;
                pm.Currency = m.Currency;
                pm.EffectiveDate = m.EffectiveDate;
                pm.MetricCode = m.MetricCode;
                pm.MetricId = m.MetricId;
                _context.Update(pm);
            }
            else
                _context.ProductMetrics.Add(CreateProductMetric(m));
        }

        public void DecideNewOrEdit(QuantityMetricDto m)
        {
            var qm = _context.QuantityMetrics.Where
                (
                    p => p.ProductId == m.ProductId
                         && p.EffectiveDate == m.EffectiveDate
                ).FirstOrDefault(); // needed tolist ? https://stackoverflow.com/questions/61052687/a-command-is-already-in-progress

            if (qm!=null) 
            {
                qm.ProductId = m.ProductId;
                qm.Value = m.Value; 
               
                qm.ProductCode = m.ProductCode;
                qm.EffectiveDate = m.EffectiveDate; 
                _context.Update(qm);
            }
            else
                _context.QuantityMetrics.Add(CreateQuantityMetric(m));
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

        private static QuantityMetric CreateQuantityMetric(QuantityMetricDto m)
        {
            return new QuantityMetric()
            {
                EffectiveDate = m.EffectiveDate,
                ProductId = m.ProductId,
                Value = m.Value,
                ProductCode = m.ProductCode,
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
           
        public async  Task<SourceDto> AddSourceAsync(SourceDto dto )
        {
            Entities.Source e = new Entities.Source() 
                                { Id = dto.Id, Description = dto.Description  };
            _context.Sources.Add(e);
            await _context.SaveChangesAsync();
            return new SourceDto(dto.Id, dto.Description);
        }

        public async Task<bool> LetProduct(LetProductDto dto)  
        {
            var ProductId = new SqlParameter("@ProductId", dto.ProductId);
            var IncreaseBy = new SqlParameter("@IncreaseBy", dto.IncreaseBy);

            ////to get this to work, you will need to change your select inside dbo.insert_department to include name in the resultset
            //var department = _context.Database.SqlQuery<QuantityMetric>(
            //    "dbo.insert_department @ProductId,@IncreaseBy ", ProductId  
            //     ).SingleOrDefault();

            await _context.SaveChangesAsync();
            return true;    
        }


            /// <summary>
            /// returns the latest by effective date product metric row 
            /// </summary>
            /// <param name="ProductCode"></param>
            /// <param name="MetricCode"></param>
            /// <returns></returns>
        public async Task<ProductMetricDto> GetProductMetricAsync(string ProductCode, string MetricCode)
        {
                ProductCode = ProductCode.ToUpper().Trim();
                MetricCode = MetricCode.ToUpper().Trim();

                return await _context
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
                       .FirstAsync();

         

        }

        Task<QuantityMetricDto> IInventoryRepository.LetProduct(LetProductDto dto)
        {
            throw new NotImplementedException();
        }

      public   async Task<QuantityMetricDto> AddQuantityMetricAsync(QuantityMetricDto dto)
        {
            QuantityMetric qm = new QuantityMetric();
            qm.ProductId = dto.ProductId;
            qm.Value = dto.Value;
            qm.ProductCode = dto.ProductCode;
            qm.EffectiveDate = dto.EffectiveDate;
            _context.Add(qm);
            await _context.SaveChangesAsync();

            Guid MetricId  = await _context.Metrics.
                                      Where(i => i.Code == Constants.QUANTITYCODE).
                                      Select(i=>i.Id).
                                      FirstOrDefaultAsync();

            // same row as in quantity metric 
           await  AddOrEditProductMetricAsync(new ProductMetricDto(
                 dto.ProductId,   
                 MetricId,
                 dto.Value,
                 dto.EffectiveDate,
                 "EUR",
                 Constants.QUANTITYCODE,
                 dto.ProductCode
            ));
                
            return await  GetQuantityMetricAsync(qm.ProductId, qm.EffectiveDate);
        }

        public async Task<QuantityMetricDto> GetQuantityMetricAsync(Guid id, DateTime EffectiveDate)
        {
            return  await _context.QuantityMetrics.
                                   Where(i => i.ProductId == id && i.EffectiveDate == EffectiveDate).
                                   Select(i =>new QuantityMetricDto(i.ProductId,i.Value,i.EffectiveDate,i.ProductCode)).
                                   SingleAsync();

        }


    }
}
