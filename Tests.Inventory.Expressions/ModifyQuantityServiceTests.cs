using Inventory.Expressions;
using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;
using Inventory.Products.Services;
using Inventory.Transactions.Dto;
using Inventory.Transactions.Repositories;
using MediatR;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Tests.Inventory.Expressions
{


    /// <summary>
    /// https://github.com/Umplify/xunit-dependency-injection/blob/main/examples/Xunit.Microsoft.DependencyInjection.ExampleTests/CalculatorTests.cs
    /// Tests transaction based  quantity modifications in Buy / Sell and Let 
    /// </summary>
    [Collection("Our Test Collection #1")]
    public class ModifyQuantityServiceTests : TestBed<TestFixture>
    {
        private const string RoomProductCode = "Room1";
        private const string Inventory = "ROOMS";
        private const string Currency = "EUR";
        private const string SourceName = "SOURCE";
        private const string ValueCode = "VALUE";
        private const string QuantityCode = "QUANTITY";
        private const string PriceCode = "PRICE";

        public ModifyQuantityServiceTests(ITestOutputHelper testOutputHelper, TestFixture fixture) :
                                  base(testOutputHelper, fixture)
        { }

        /// <summary>
        /// </summary>
        /// <returns></returns>
      [Fact]
      public async Task TestOnSellOneItShouldCreateOneWithQuantityDecremented()
        {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _transrepo = _fixture.GetService<ITransactionRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            var _modificationService = _fixture.GetService<IModifyQuantityService>(_testOutputHelper)!;
            
            _repo.EmptyDB();

         
            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;

            var _datarepo = _fixture.GetService<IDataPrepareRepository>(_testOutputHelper)!;
            var templateId = await _datarepo.RoomsPrepareAsync();
            var TransactionId = (await _transrepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(), "", DateTime.Now, templateId, null))).Id;



            var firstDate = new DateTime(2022, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 1, firstDate, TransactionId, 1, false);
            var qm = await _repo.AddQuantityMetricAsync(quantityMetricDto);
            var secondDate = new DateTime(2023, 1, 1, 1, 1, 1);

            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =    1,
                        EffectiveFrom = secondDate,
                        EffectiveTo = secondDate,
                        ModificationType   = ModificationType.Sell
                    }
               });

            var qms = await _repo.GetQuantityMetricsAsync();
            Assert.Equal(0, qms[1].Value);
            Assert.Equal(2, qms.Count);
        }

      [Fact]
      public async Task TestOnSellOneItShouldNotCreateOneWhenQuantityIsZero()
        {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            var _modificationService = _fixture.GetService<IModifyQuantityService>(_testOutputHelper)!;
            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            var _transrepo = _fixture.GetService<ITransactionRepository>(_testOutputHelper)!;

            var _datarepo = _fixture.GetService<IDataPrepareRepository>(_testOutputHelper)!;
            var templateId = await _datarepo.RoomsPrepareAsync();
            var TransactionId = (await _transrepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(), "", DateTime.Now, templateId, null))).Id;



            var firstDate = new DateTime(2022, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 0,  firstDate, TransactionId, 1, false);
            var qm = await _repo.AddQuantityMetricAsync(quantityMetricDto);

            var secondDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =    1,
                        EffectiveFrom = secondDate,
                        EffectiveTo = secondDate,
                        ModificationType   = ModificationType.Sell
                    }
               });

            var qms = await _repo.GetQuantityMetricsAsync();
            Assert.Single(qms);
        }

      [Fact]
      public async Task TestOnSellQuantityWithHigherValueThanPostEffectiveDateShouldAbort()
        {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            var _modificationService = _fixture.GetService<IModifyQuantityService>(_testOutputHelper)!;

            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;

            var _datarepo = _fixture.GetService<IDataPrepareRepository>(_testOutputHelper)!;

            var templateId = await _datarepo.RoomsPrepareAsync();
            var _transrepo = _fixture.GetService<ITransactionRepository>(_testOutputHelper)!;
            var TransactionId = (await _transrepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(), "", DateTime.Now, templateId, null))).Id;



            var firstDate = new DateTime(2021, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId,10, firstDate, TransactionId, 1, false);
            var qm = await _repo.AddQuantityMetricAsync(quantityMetricDto);

            var secondDate = new DateTime(2025, 1, 1, 1, 1, 1);
            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(productId, 5, secondDate, TransactionId, 1, false);
            var qm2 = await _repo.AddQuantityMetricAsync(quantityMetricDto2);

            var thirdDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =  8,
                        EffectiveFrom = thirdDate,
                        EffectiveTo = thirdDate,
                        ModificationType   = ModificationType.Sell
                    }
               });
            var qms = await _repo.GetQuantityMetricsAsync();
            Assert.Equal(2, qms.Count);

        }

      [Fact]
      public async Task TestOnSellQuantityWillDecrementPostEffectiveDate()
        {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            var _modificationService = _fixture.GetService<IModifyQuantityService>(_testOutputHelper)!;

            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            var _transrepo = _fixture.GetService<ITransactionRepository>(_testOutputHelper)!;
          
            
            var _datarepo = _fixture.GetService<IDataPrepareRepository>(_testOutputHelper)!;
            var templateId = await _datarepo.RoomsPrepareAsync();
            var TransactionId = (await _transrepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(), "", DateTime.Now, templateId, null))).Id;


            var firstDate = new DateTime(2021, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 10, firstDate, TransactionId, 1, false);
            var qm = await _repo.AddQuantityMetricAsync(quantityMetricDto);

            var secondDate = new DateTime(2025, 1, 1, 1, 1, 1);
            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(productId, 5, secondDate, TransactionId, 1, false);
            var qm2 = await _repo.AddQuantityMetricAsync(quantityMetricDto2);


            var thirdDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =  3,
                        EffectiveFrom = thirdDate,
                        EffectiveTo = thirdDate,
                        ModificationType   = ModificationType.Sell
                    }
               });

            var qms = (await _repo.GetQuantityMetricsAsync()).OrderByDescending(p => p.EffectiveDate).ToList();

            Assert.Equal(10, qms[2].Value);
            Assert.Equal(7, qms[1].Value);
            Assert.Equal(2, qms[0].Value);
            Assert.Equal(3, qms.Count);
        }

      [Fact]
      public async Task TestChainUpdatesOnRecordsAfterInsert()
        {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            var _modificationService = _fixture.GetService<IModifyQuantityService>(_testOutputHelper)!;

            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            var _transrepo = _fixture.GetService<ITransactionRepository>(_testOutputHelper)!;
         
            var _datarepo = _fixture.GetService<IDataPrepareRepository>(_testOutputHelper)!;
            var templateId = await _datarepo.RoomsPrepareAsync();
            var TransactionId = (await _transrepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(), "", DateTime.Now, templateId, null))).Id;

            var firstDate = new DateTime(2024, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 0,  firstDate, TransactionId, 1, false);
            var qm = await _repo.AddQuantityMetricAsync(quantityMetricDto);

            var secondDate = new DateTime(2025, 1, 1, 1, 1, 1);
            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(productId, 5,  secondDate, TransactionId, 1, false);
            var qm2 = await _repo.AddQuantityMetricAsync(quantityMetricDto2);

            var thirdDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =   5,
                        EffectiveFrom = thirdDate,
                        EffectiveTo = thirdDate,
                        ModificationType   = ModificationType.Buy
                    }
               });
            var qms = (await _repo.GetQuantityMetricsAsync()).OrderByDescending(p=>p.EffectiveDate).ToList();

            Assert.Equal(5, qms[2].Value);
            Assert.Equal(5, qms[1].Value);
            Assert.Equal(10, qms[0].Value);
            Assert.Equal(3, qms.Count);
        }

      [Fact]
      public async Task TestOverlappingInBoundDtosWillNotSave()
      {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            var _modificationService = _fixture.GetService<IModifyQuantityService>(_testOutputHelper)!;

            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            var _transrepo = _fixture.GetService<ITransactionRepository>(_testOutputHelper)!;
          
            var _datarepo = _fixture.GetService<IDataPrepareRepository>(_testOutputHelper)!;
            var templateId = await _datarepo.RoomsPrepareAsync();
            var TransactionId = (await _transrepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(), "", DateTime.Now, templateId, null))).Id;

            var firstDate = new DateTime(2000, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 1, firstDate, TransactionId, 1, false);
            await _repo.AddQuantityMetricAsync(quantityMetricDto);

            var firstEntryDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate,
                        EffectiveTo = firstEntryDate.AddDays(300),
                        ModificationType   = ModificationType.Let
                    } ,
                          new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate.AddDays(-10),
                        EffectiveTo = firstEntryDate.AddDays(100),
                        ModificationType   = ModificationType.Let
                    }
               });

            var qms = await _repo.GetQuantityMetricsAsync();
            Assert.Single(qms);  

        }

      [Fact]
      public async Task TestLetWithEffectiveToOverlappingWithExistingWillNotSave()
        {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            var _modificationService = _fixture.GetService<IModifyQuantityService>(_testOutputHelper)!;

            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            var _transrepo = _fixture.GetService<ITransactionRepository>(_testOutputHelper)!;


            var _datarepo = _fixture.GetService<IDataPrepareRepository>(_testOutputHelper)!;
            var templateId = await _datarepo.RoomsPrepareAsync();
            var TransactionId = (await _transrepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(), "", DateTime.Now, templateId, null))).Id;

            var firstDate = new DateTime(2000, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 1, firstDate, TransactionId, 1, false);
            await _repo.AddQuantityMetricAsync(quantityMetricDto);

            var firstEntryDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate,
                        EffectiveTo = firstEntryDate.AddDays(300),
                        ModificationType   = ModificationType.Let
                    }
               });

            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =  1,
                        EffectiveFrom = firstEntryDate.AddDays(-1),
                        EffectiveTo = firstEntryDate.AddDays(3),
                        ModificationType   = ModificationType.Let
                    }
               });


            var qms = await _repo.GetQuantityMetricsAsync();
            Assert.Equal(3, qms.Count);

        }


        [Fact]
        public async Task TestOnMultipleLetwithQuantityTwoShouldAllow()
      {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            var _modificationService = _fixture.GetService<IModifyQuantityService>(_testOutputHelper)!;

            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            var _transrepo = _fixture.GetService<ITransactionRepository>(_testOutputHelper)!;
            var _datarepo = _fixture.GetService<IDataPrepareRepository>(_testOutputHelper)!;

            var templateId = await _datarepo.RoomsPrepareAsync();
            var TransactionId = (await _transrepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(), "", DateTime.Now, templateId, null))).Id;

            var firstDate = new DateTime(2000, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 2, firstDate, TransactionId, 1, false);
            await _repo.AddQuantityMetricAsync(quantityMetricDto);

            var firstEntryDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate,
                        EffectiveTo = firstEntryDate.AddDays(300),
                        ModificationType   = ModificationType.Let
                    }
               });

            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =  1,
                        EffectiveFrom = firstEntryDate.AddDays(-1),
                        EffectiveTo = firstEntryDate.AddDays(3),
                        ModificationType   = ModificationType.Let
                    }
               });


            var qms = await _repo.GetQuantityMetricsAsync();
            Assert.Equal(5, qms.Count);

        }

        [Fact]
        public async Task TestOnMultipleLetWithinTheSameInboundListwithQuantityTwoShouldAllow()
      {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            var _modificationService = _fixture.GetService<IModifyQuantityService>(_testOutputHelper)!;

            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            var _transrepo = _fixture.GetService<ITransactionRepository>(_testOutputHelper)!;
          
            var _datarepo = _fixture.GetService<IDataPrepareRepository>(_testOutputHelper)!;
            var templateId = await _datarepo.RoomsPrepareAsync();
            var TransactionId = (await _transrepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(), "", DateTime.Now, templateId, null))).Id;

            var firstDate = new DateTime(2000, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 2, firstDate, TransactionId, 1, false);
            await _repo.AddQuantityMetricAsync(quantityMetricDto);

            var firstEntryDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate,
                        EffectiveTo = firstEntryDate.AddDays(300),
                        ModificationType   = ModificationType.Let
                    },
                      new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =  1,
                        EffectiveFrom = firstEntryDate.AddDays(-1),
                        EffectiveTo = firstEntryDate.AddDays(3),
                        ModificationType   = ModificationType.Let
                    }
               });

            var qms = await _repo.GetQuantityMetricsAsync();
            Assert.Equal(5, qms.Count);

        }

        [Fact]
        public async Task TestOnSellWithNoRecordShouldAbort()
        {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            var _modificationService = _fixture.GetService<IModifyQuantityService>(_testOutputHelper)!;

            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;

       

            var firstEntryDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await _modificationService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       productId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate,
                        EffectiveTo = firstEntryDate.AddDays(300),
                        ModificationType   = ModificationType.Sell
                    },
                   
               });

            var qms = await _repo.GetQuantityMetricsAsync();
            Assert.Empty(qms);

        }

    }
}
 