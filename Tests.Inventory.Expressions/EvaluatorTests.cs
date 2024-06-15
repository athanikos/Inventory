using Expressions;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;
using MediatR;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Inventory.Products.Contracts;
using Inventory.Products;
using System.Reflection;

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


        /// <summary>
        /// QUANTITY(ADA) test 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestEvaluatorQuantityOfProduct()
        {
            string expression = "QUANTITY(ADA)";

            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            //  todo extract string to const
            // todo extract preparation steps , services and empty db 
            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), "CRYPTO"))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), "SOURCE"))).Id;
            var metricId = (await _repo.AddMetricAsync(NewMetricDto(sourceId, "QUANTITY"    ))).Id;

            ProductDto prodDto = NewProductDto(InventoryId);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;

            await _repo.AddOrEditProductMetric(NewProdctMetricDto(metricId, productId, 1, "EUR", "ADA", "QUANTITY"));
            Evaluator instance = new Evaluator(_mediator, expression);
            string result = await instance.Execute();
            Assert.Equal("1.000000",result);
        }

        /// <summary>
        /// QUANTITY(ADA) * PRICE(ADA) test 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestEvaluatorBalanceOfProduct()
        {
            string expression = "QUANTITY(ADA) * PRICE(ADA)";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            //  todo extract string to const
            // todo extract preparation steps , services and empty db 

            _repo.EmptyDB();
            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), "CRYPTO"))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), "SOURCE"))).Id;
          
            
            var quantityId = (await _repo.AddMetricAsync(NewMetricDto(sourceId,"QUANTITY"))).Id;
            var priceId = (await _repo.AddMetricAsync(NewMetricDto(sourceId, "PRICE"))).Id;


            ProductDto prodDto = NewProductDto(InventoryId);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
           
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(quantityId, productId,1,"EUR","ADA","QUANTITY"));
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(priceId, productId, 5, "EUR", "ADA", "PRICE"));


            Evaluator instance = new Evaluator(_mediator, expression);
            string result = await instance.Execute();
            Assert.Equal("1.000000*5.000000", result);
        }

        private static ProductMetricDto NewProdctMetricDto(Guid metricId, Guid productId,
            int quantity, string currency, string productCode, string metricCode   )
        {
            return new ProductMetricDto(productId, metricId, quantity, DateTime.MinValue, currency, productCode, metricCode);
        }

        private static  ProductDto NewProductDto(Guid InventoryId )
        {
            return new ProductDto(Guid.NewGuid(),
                                         "",
                                         "ADA",
                                         InventoryId,
                                         new List<ProductMetricDto>());
        }
        private static MetricDto NewMetricDto(Guid SourceId,string MetricCode )
        {
            return new MetricDto(Guid.NewGuid(), "", MetricCode, SourceId);
        }
    }
}
