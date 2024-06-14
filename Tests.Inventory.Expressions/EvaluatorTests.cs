using Expressions;
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
    public class EvaluatorTests : TestBed<TestFixture>
    {

        public EvaluatorTests(ITestOutputHelper testOutputHelper, TestFixture fixture) :
            base(testOutputHelper, fixture)
        {

        }

        private void EmptyDB()
        {

        }


        [Fact]
        public async Task TestComputeTokens()
        {





            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), "CRYPTO"))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), "SOURCE"))).Id;
            var metricId = (await _repo.AddMetricAsync(NewMetricDto(sourceId))).Id;

            ProductDto prodDto = NewProductDto(InventoryId);

            var productId = (await _repo.AddProductAsync(prodDto)).Id;


            await _repo.AddOrEditProductMetric(NewProdctMetricDto(metricId, productId));

            Evaluator instance = new Evaluator(_mediator, "QUANTITY(ADA)");
            string result = await instance.Execute();

            Assert.Equal(1, 1);


        }

        private static ProductMetricDto NewProdctMetricDto(Guid metricId, Guid productId)
        {
            return new ProductMetricDto(productId, metricId, 1, DateTime.MinValue, "EUR", "ADA", "QUANTITY");
        }

        private static  ProductDto NewProductDto(Guid InventoryId )
        {
            return new ProductDto(Guid.NewGuid(),
                                         "",
                                         "ADA",
                                         InventoryId,
                                         new List<ProductMetricDto>());
        }
        private static MetricDto NewMetricDto(Guid SourceId )
        {
            return new MetricDto(Guid.NewGuid(), "", "QUANTITY", SourceId);
        }
    }
}
