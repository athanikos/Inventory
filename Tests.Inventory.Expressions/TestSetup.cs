using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;
using Inventory.Products.Services;
using Inventory.Transactions.Dto;
using Inventory.Transactions.Repositories;
using Tests.Inventory.Expressions;
using Xunit.Abstractions;

namespace Tests.Inventory
{
    public class TestSetup
    {
        public Guid ProductId { get; set; }
        public Guid TemplateId { get; set; }
        public Guid TransactionId { get; set; }
        public required ITransactionRepository TransactionRepo { get; set; }
        public required IInventoryRepository InventoryRepo { get; set; }
        public required IModifyQuantityService ModifyQuantityService { get; set; }

        private const string RoomProductCode = "Room1";
        private const string Inventory = "ROOMS";
        private const string Currency = "EUR";
        private const string SourceName = "SOURCE";
        private const string ValueCode = "VALUE";
        private const string QuantityCode = "QUANTITY";
        private const string PriceCode = "PRICE";

        public static async Task<TestSetup> Setup(ITestOutputHelper _testOutputHelper, TestFixture fixture)
        {
            TestSetup output = new TestSetup() 
            { 
                InventoryRepo = fixture.GetService<IInventoryRepository>(_testOutputHelper)!, 
                TransactionRepo = fixture.GetService<ITransactionRepository>(_testOutputHelper)!, 
                ModifyQuantityService = fixture.GetService<IModifyQuantityService>(_testOutputHelper)! 
            };

            output.InventoryRepo.EmptyDB();
            await output.TransactionRepo.EmptyDB();
            var InventoryId = (await output.InventoryRepo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await output.InventoryRepo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await output.InventoryRepo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            output.TemplateId = await output.TransactionRepo.RoomsPrepareAsync();
            output.TransactionId = (await output.TransactionRepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(), "", DateTime.Now, output.TemplateId, null))).Id;
            output.ProductId = (await output.InventoryRepo.AddProductAsync(prodDto)).Id;
          
            return output;
        }
    }
}
