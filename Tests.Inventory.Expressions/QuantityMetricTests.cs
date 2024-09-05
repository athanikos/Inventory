using Expressions;
using Inventory.Expressions;
using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Tests.Inventory.Expressions
{


    /// <summary>
    /// https://github.com/Umplify/xunit-dependency-injection/blob/main/examples/Xunit.Microsoft.DependencyInjection.ExampleTests/CalculatorTests.cs
    /// </summary>
    [Collection("Our Test Collection #1")]
    public class QuantityMetricTests : TestBed<TestFixture>
    {
        private const string RoomProductCode = "Room1";
        private const string Inventory = "ROOMS";
        private const string Currency = "EUR";
        private const string SourceName = "SOURCE";
        private const string ValueCode = "VALUE";
        private const string QuantityCode = "QUANTITY";
        private const string PriceCode = "PRICE";

        public QuantityMetricTests(ITestOutputHelper testOutputHelper, TestFixture fixture) :
                                  base(testOutputHelper, fixture)
        { }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestOnAddQuantityMetricItShouldAddtoProductMetricAsWell()
        {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 1, RoomProductCode, DateTime.MinValue);

            var qm = await _repo.AddQuantityMetricAsync(quantityMetricDto);

            var pm = await _repo.GetProductMetricAsync(productId, DateTime.MinValue);
            Assert.NotNull(pm);
            Assert.NotNull(qm);
        }

        [Fact]
        public async Task TestUniqueConstraintOnMinutesDifferenceTwoRecordsAreInserted()
        {

            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;

            DateTime dateWithMinutes = new DateTime(2020, 1, 1, 1, 1, 0);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 1, RoomProductCode, dateWithMinutes);

            var qm1 = await _repo.AddQuantityMetricAsync(quantityMetricDto);
            DateTime dateWithMinutes2 = new DateTime(2020, 1, 1, 1, 12, 0);

            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(productId, 1, RoomProductCode, dateWithMinutes2);
            await _repo.AddQuantityMetricAsync(quantityMetricDto2);

            var qms = await _repo.GetQuantityMetricsAsync();
            Assert.Equal(2, qms.Count);
        }

        [Fact]
        public async Task TestUniqueConstraintSameRecordIsNotInsertedTwice()
        {

            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;

            DateTime dateWithMinutes = new DateTime(2020, 1, 1, 1, 12, 0);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 1, RoomProductCode, dateWithMinutes);

            var qm1 = await _repo.AddQuantityMetricAsync(quantityMetricDto);
            DateTime dateWithMinutes2 = new DateTime(2020, 1, 1, 1, 12, 0);

            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(productId, 1, RoomProductCode, dateWithMinutes2);
            try
            {
                await _repo.AddQuantityMetricAsync(quantityMetricDto2);
            }
            catch (Exception ex) // throws exeeption on unique constraint 
            {
            }
            var qms = await _repo.GetQuantityMetricsAsync();
            Assert.Single(qms);
        }

        [Fact]
        public async Task TestTwoIdenticalRecordsAddedToContextNoneIsSaved()
        {

            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;

            _repo.EmptyDB();
            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Inventory))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, Constants.QUANTITYCODE))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            DateTime dateWithMinutes = new DateTime(2020, 1, 1, 1, 12, 0);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 1, RoomProductCode, dateWithMinutes);

            _repo.AddQuantityMetric(quantityMetricDto);
            _repo.AddQuantityMetric(quantityMetricDto); // fails on context add not even on saves 

            try { await _repo.SaveChangesAsync(); }
            catch (Exception ex) { }

            var qms = await _repo.GetQuantityMetricsAsync();
            Assert.Empty(qms);
        }

        [Fact]
        public async Task TestMultipleIntervalsAreNotSavedWhenAtleastOneIsNotAvailable()
        {
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _ProductsDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;

            //todo implement  insert one qa for some interval 
            // save 
            // then attempt to add 3 intervals with 1 overalapping with then one above 



            await _repo.GetQuantityMetricsAsync();
        }
    }

}