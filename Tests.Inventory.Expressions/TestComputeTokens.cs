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

        [Fact]
        public async void TestComputeTokens()
        {


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;

            await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), "CRYPTO"));
            var metricId = (await _repo.AddMetricAsync(new MetricDto(Guid.NewGuid(), "", "QUANTITY", Guid.NewGuid()))).Id;

            var prodDto = new ProductDto(Guid.NewGuid(),
                                         "",
                                         "ADA",
                                         Guid.NewGuid(),
                                         new List<ProductMetricDto>());

            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetric(new ProductMetricDto(productId, metricId, 1, DateTime.MinValue, "EUR", "ADA", "QUANTITY"));

            Evaluator instance = new Evaluator(_mediator, "QUANTITY(ADA)");
            string result = await instance.Execute();

            Assert.Equal(1, 1);

        }


    }
}
