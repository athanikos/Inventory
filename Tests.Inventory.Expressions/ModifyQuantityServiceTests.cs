using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
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
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);


            var firstDate = new DateTime(2022, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1, firstDate, output.TransactionId, 1, false);
            var qm = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);
            var secondDate = new DateTime(2023, 1, 1, 1, 1, 1);

            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               [
                    new ModifyQuantityDto()
                    {
                        ProductId =       output.ProductId,
                        Diff =    1,
                        EffectiveFrom = secondDate,
                        EffectiveTo = secondDate,
                        ModificationType   = ModificationType.Sell
                    }
               ]);

            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Equal(0, qms[1].Value);
            Assert.Equal(2, qms.Count);
        }

        [Fact]
        public async Task TestOnSellOneItShouldNotCreateOneWhenQuantityIsZero()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);



            var firstDate = new DateTime(2022, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 0, firstDate, output.TransactionId, 1, false);
            var qm = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);

            var secondDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               inboundQuantities: new List<ModifyQuantityDto>()
               {
                    new ()
                    {
                        ProductId =       output.ProductId,
                        Diff =    1,
                        EffectiveFrom = secondDate,
                        EffectiveTo = secondDate,
                        ModificationType   = ModificationType.Sell
                    }
               });

            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Single(qms);
        }

        [Fact]
        public async Task TestOnSellQuantityWithHigherValueThanPostEffectiveDateShouldAbort()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);





            var firstDate = new DateTime(2021, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 10, firstDate, output.TransactionId, 1, false);
            var qm = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);

            var secondDate = new DateTime(2025, 1, 1, 1, 1, 1);
            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 5, secondDate, output.TransactionId, 1, false);
            var qm2 = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto2);

            var thirdDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               [
                    new ModifyQuantityDto()
                    {
                        ProductId =       output.ProductId,
                        Diff =  8,
                        EffectiveFrom = thirdDate,
                        EffectiveTo = thirdDate,
                        ModificationType   = ModificationType.Sell
                    }
               ]);
            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Equal(2, qms.Count);

        }

        [Fact]
        public async Task TestOnSellQuantityWillDecrementPostEffectiveDate()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);

            var firstDate = new DateTime(2021, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 10, firstDate, output.TransactionId, 1, false);
            var qm = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);

            var secondDate = new DateTime(2025, 1, 1, 1, 1, 1);
            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 5, secondDate, output.TransactionId, 1, false);
            var qm2 = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto2);


            var thirdDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ()
                    {
                        ProductId =       output.ProductId,
                        Diff =  3,
                        EffectiveFrom = thirdDate,
                        EffectiveTo = thirdDate,
                        ModificationType   = ModificationType.Sell
                    }
               });

            var qms = (await output.InventoryRepo.GetQuantityMetricsAsync()).OrderByDescending(p => p.EffectiveDate).ToList();

            Assert.Equal(10, qms[2].Value);
            Assert.Equal(7, qms[1].Value);
            Assert.Equal(2, qms[0].Value);
            Assert.Equal(3, qms.Count);
        }

        [Fact]
        public async Task TestChainUpdatesOnRecordsAfterInsert()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);



            var firstDate = new DateTime(2024, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 0, firstDate,
                output.TransactionId, 1, false);
            var qm = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);

            var secondDate = new DateTime(2025, 1, 1, 1, 1, 1);
            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 5, secondDate,
                output.TransactionId, 1, false);
            var qm2 = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto2);

            var thirdDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ModifyQuantityDto()
                    {
                        ProductId =       output.ProductId,
                        Diff =   5,
                        EffectiveFrom = thirdDate,
                        EffectiveTo = thirdDate,
                        ModificationType   = ModificationType.Buy
                    }
               });
            var qms = (await output.InventoryRepo.GetQuantityMetricsAsync()).OrderByDescending(p => p.EffectiveDate).ToList();

            Assert.Equal(5, qms[2].Value);
            Assert.Equal(5, qms[1].Value);
            Assert.Equal(10, qms[0].Value);
            Assert.Equal(3, qms.Count);
        }

        [Fact]
        public async Task TestOverlappingInBoundDtosWillNotSave()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);



            var firstDate = new DateTime(2000, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1, firstDate, output.TransactionId, 1, false);
            await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);

            var firstEntryDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               [
                    new ModifyQuantityDto()
                    {
                        ProductId =       output.ProductId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate,
                        EffectiveTo = firstEntryDate.AddDays(300),
                        ModificationType   = ModificationType.Let
                    } ,
                          new ModifyQuantityDto()
                    {
                        ProductId =       output.ProductId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate.AddDays(-10),
                        EffectiveTo = firstEntryDate.AddDays(100),
                        ModificationType   = ModificationType.Let
                    }
               ]);

            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Single(qms);

        }

        [Fact]
        public async Task TestLetWithEffectiveToOverlappingWithExistingWillNotSave()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);



            var firstDate = new DateTime(2000, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1, firstDate, output.TransactionId, 1, false);
            await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);

            var firstEntryDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               [
                    new ModifyQuantityDto()
                    {
                        ProductId =       output.ProductId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate,
                        EffectiveTo = firstEntryDate.AddDays(300),
                        ModificationType   = ModificationType.Let
                    }
               ]);

            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ()
                    {
                        ProductId =       output.ProductId,
                        Diff =  1,
                        EffectiveFrom = firstEntryDate.AddDays(-1),
                        EffectiveTo = firstEntryDate.AddDays(3),
                        ModificationType   = ModificationType.Let
                    }
               });


            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Equal(3, qms.Count);

        }


        [Fact]
        public async Task TestOnMultipleLetwithQuantityTwoShouldAllow()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);



            var firstDate = new DateTime(2000, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 2, firstDate, output.TransactionId, 1, false);
            await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);

            var firstEntryDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ()
                    {
                        ProductId =       output.ProductId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate,
                        EffectiveTo = firstEntryDate.AddDays(300),
                        ModificationType   = ModificationType.Let
                    }
               });

            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new()
                    {
                        ProductId =       output.ProductId,
                        Diff =  1,
                        EffectiveFrom = firstEntryDate.AddDays(-1),
                        EffectiveTo = firstEntryDate.AddDays(3),
                        ModificationType   = ModificationType.Let
                    }
               });


            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Equal(5, qms.Count);

        }

        [Fact]
        public async Task TestOnMultipleLetWithinTheSameInboundListwithQuantityTwoShouldAllow()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);



            var firstDate = new DateTime(2000, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 2, firstDate, output.TransactionId, 1, false);
            await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);

            var firstEntryDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ()
                    {
                        ProductId =       output.ProductId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate,
                        EffectiveTo = firstEntryDate.AddDays(300),
                        ModificationType   = ModificationType.Let
                    },
                      new ()
                    {
                        ProductId =       output.ProductId,
                        Diff =  1,
                        EffectiveFrom = firstEntryDate.AddDays(-1),
                        EffectiveTo = firstEntryDate.AddDays(3),
                        ModificationType   = ModificationType.Let
                    }
               });

            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Equal(5, qms.Count);

        }

        [Fact]
        public async Task TestOnSellWithNoRecordShouldAbort()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);

            var firstEntryDate = new DateTime(2023, 1, 1, 1, 1, 1);
            await output.ModifyQuantityService.ModifyQuantityMetricsAsync(
               new List<ModifyQuantityDto>()
               {
                    new ()
                    {
                        ProductId =       output.ProductId,
                        Diff =   1,
                        EffectiveFrom = firstEntryDate,
                        EffectiveTo = firstEntryDate.AddDays(300),
                        ModificationType   = ModificationType.Sell
                    },

               });

            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Empty(qms);

        }


        [Fact]
        public async Task TestOnOnCancellationNoRecordShouldBeReturned()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);



        }
    }
    }
 