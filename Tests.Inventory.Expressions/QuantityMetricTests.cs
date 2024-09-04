using Expressions;
using Inventory.Expressions;
using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;
using MediatR;
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
                                  base(testOutputHelper, fixture)  {    }
        
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
                var metricId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId,Constants.QUANTITYCODE))).Id;
                ProductDto prodDto = ProductDto.NewProductDto(InventoryId, RoomProductCode);
                var productId = (await _repo.AddProductAsync(prodDto)).Id;
                var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(productId, 1, RoomProductCode,DateTime.MinValue);
       
                var qm =   await _repo.AddQuantityMetricAsync(quantityMetricDto);
                var pm =    await _repo.GetProductMetricAsync(productId, DateTime.MinValue);
                Assert.NotNull(pm);
                Assert.NotNull(qm);
        }



    }
}
