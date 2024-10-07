using Inventory.Defaults.Services;
using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;
using Inventory.Products.Services;
using Inventory.Transactions.Dto;
using Inventory.Transactions.Repositories;
using Inventory.Transactions.Services;
using Xunit.Abstractions;

namespace Tests.Inventory
{
    public class TestSetup
    {
        public Guid ProductId { get; set; }
        public Guid TemplateId { get; set; }
        public Guid TransactionId { get; private set; }
        public Guid QuantityId  { get; private set; }   

        public required ITransactionRepository TransactionRepo { get; init; }
        public required IInventoryRepository InventoryRepo { get; init; }
        public required IModifyQuantityService ModifyQuantityService { get; init; }
        public required IInventoryService InventoryService { get; init; }
        public required ITransactionService TransactionService { get; init; }
        public required IConfigurationService ConfigurationService { get; set; }
        
        private const string RoomProductCode = "Room1";
        private const string Inventory = "ROOMS";
        private const string Currency = "EUR";
        private const string SourceName = "SOURCE";
        private const string ValueCode = "VALUE";
        private const string QuantityCode = "QUANTITY";
        private const string PriceCode = "PRICE";

        public static async Task<TestSetup> ClearDb(ITestOutputHelper testOutputHelper, TestFixture fixture)
        {
            TestSetup output = new TestSetup()
            {
                InventoryRepo = fixture.GetService<IInventoryRepository>(testOutputHelper)!,
                TransactionRepo = fixture.GetService<ITransactionRepository>(testOutputHelper)!,
                ModifyQuantityService = fixture.GetService<IModifyQuantityService>(testOutputHelper)!,
                ConfigurationService = fixture.GetService<IConfigurationService>(testOutputHelper)!,
                InventoryService = fixture.GetService<IInventoryService>(testOutputHelper)!,
                TransactionService =fixture.GetService<ITransactionService>(testOutputHelper)!, 
            };
            output.InventoryRepo.EmptyDb();
            await output.TransactionRepo.EmptyDb();
            return output;
        }

        public static async Task<TestSetup> Setup(ITestOutputHelper testOutputHelper, TestFixture fixture, string productCode = RoomProductCode )
        {

            TestSetup output = new TestSetup()
            {
                InventoryRepo = fixture.GetService<IInventoryRepository>(testOutputHelper)!,
                TransactionRepo = fixture.GetService<ITransactionRepository>(testOutputHelper)!,
                ModifyQuantityService = fixture.GetService<IModifyQuantityService>(testOutputHelper)!,
                ConfigurationService = fixture.GetService<IConfigurationService>(testOutputHelper)!,
                InventoryService = fixture.GetService<IInventoryService>(testOutputHelper)!,
                TransactionService= fixture.GetService<ITransactionService>(testOutputHelper)!
            };
            output.InventoryRepo.EmptyDb();
            await output.TransactionRepo.EmptyDb();
            output.ConfigurationService.EmptyDb();
          
            var inventoryId = (await output.InventoryRepo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), 
                Inventory, string.Empty))).Id;
            var sourceId = (await output.InventoryRepo.AddSourceAsync(new SourceDto(Guid.NewGuid(), 
                SourceName))).Id;
            var metricId = (await output.InventoryRepo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, 
                Constants.Quantitycode))).Id;
            
            ProductDto prodDto = ProductDto.NewProductDto(inventoryId, productCode);
            output.TemplateId = await output.TransactionRepo.RoomsPrepareAsync();
        
            output.TransactionId = (await output.TransactionRepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(),
                "", DateTime.Now, output.TemplateId, null))).Id;
       
            output.ProductId = (await output.InventoryRepo.AddProductAsync(prodDto)).Id;
            
            output.QuantityId = metricId;
            return output;
        }


    }
}
