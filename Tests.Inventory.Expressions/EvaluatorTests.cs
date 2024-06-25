using Expressions;
using Inventory.Expressions;
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
        private const string ADAProductCode = "ADA";
        private const string XRPProductCode = "XRP";
        private const string Crypto = "CRYPTO";
        private const string Currency = "EUR";
        private const string SourceName = "Source";
        private const string ValueCode = "VALUE";
        private const string QuantityCode = "QUANTITY";
        private const string PriceCode = "PRICE";


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
            var _expressionDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;


            //  todo extract string to const
            // todo extract preparation steps , services and empty db 
            _repo.EmptyDB();

            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Crypto))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(), SourceName))).Id;
            var metricId = (await _repo.AddMetricAsync(NewMetricDto(sourceId, QuantityCode    ))).Id;

            ProductDto prodDto = NewProductDto(InventoryId, ADAProductCode  );
            var productId = (await _repo.AddProductAsync(prodDto)).Id;

            await _repo.AddOrEditProductMetric(NewProdctMetricDto(metricId, productId, 1, Currency, ADAProductCode, QuantityCode));
            Evaluator instance = new Evaluator(_mediator, _expressionDbContext  );
            EvaluatorResult result = await instance.Execute(InventoryId, expression);
            Assert.Equal(1, decimal.Parse(result.Result));
        }

        /// <summary>
        /// QUANTITY(ADA) * PRICE(ADA) test 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestEvaluatorBalanceOfProduct()
        {

            //                   QUANTITY([ADA]) * PRICE([ADA])
            string expression = "QUANTITY([ADA]) * PRICE([ADA])";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            //  todo extract string to const
            // todo extract preparation steps , services and empty db 

            _repo.EmptyDB();
          
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2; 


            var quantityId = (await _repo.AddMetricAsync(NewMetricDto(sourceId, QuantityCode))).Id;
            var priceId = (await _repo.AddMetricAsync(NewMetricDto(sourceId, PriceCode))).Id;
            ProductDto prodDto = NewProductDto(InventoryId,ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(quantityId, productId, 1, Currency, ADAProductCode, QuantityCode));
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(priceId, productId, 5, Currency, ADAProductCode, PriceCode));


            Evaluator instance = new Evaluator(_mediator, _expressionDbContext);
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal(5, decimal.Parse(result.Result));
        }

        private static async Task<Tuple<Guid,Guid>> SetupInventoryAndSource(IInventoryRepository _repo)
        {
            var InventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Crypto))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(),SourceName))).Id;
            return new Tuple<Guid, Guid>(InventoryId, sourceId);    
        }

        [Fact]
        public async Task TestEvaluatorComplexSingleProduct()
        {
            string expression = "SUM(VALUE([ADA],LATEST))";

            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;


            //  todo extract string to const
            // todo extract preparation steps , services and empty db 

            _repo.EmptyDB();

            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(NewMetricDto(sourceId, ValueCode))).Id;
     
            
            ProductDto prodDto = NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(valueId, productId, 10, Currency, ADAProductCode,ValueCode));
      

            Evaluator instance = new Evaluator(_mediator, _expressionDbContext  );
            var result = await instance.Execute(InventoryId,expression);
            Assert.Equal(10, decimal.Parse(result.Result));
        }


        [Fact]
        public async Task TestEvaluatorComplexALLProduct()
        {
            string expression = "SUM(VALUE([ALL],LATEST))";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;

            //  todo extract string to const
            // todo extract preparation steps , services and empty db 

            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(NewMetricDto(sourceId, ValueCode))).Id;


            ProductDto prodDto = NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(valueId, productId, 10, Currency, ADAProductCode, ValueCode));

            prodDto = NewProductDto(InventoryId, XRPProductCode);
            productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(valueId, productId, 100, Currency, XRPProductCode, ValueCode));



            Evaluator instance = new Evaluator(_mediator, _expressionDbContext  );
            var result = await instance.Execute(InventoryId,expression);
            Assert.Equal(110, decimal.Parse(result.Result));
        }

        [Fact]
        public async Task TestEvaluatorComplexTwoProducts()
        {
            string expression = "SUM(VALUE([ADA,XRP],LATEST))";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;

            //  todo extract string to const
            // todo extract preparation steps , services and empty db 

            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(NewMetricDto(sourceId, ValueCode))).Id;


            ProductDto prodDto = NewProductDto(InventoryId,ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(valueId, productId, 10, Currency, ADAProductCode, ValueCode));

            prodDto = NewProductDto(InventoryId, XRPProductCode);
            productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(valueId, productId, 11, Currency, XRPProductCode, ValueCode));


            Evaluator instance = new Evaluator(_mediator, _expressionDbContext  );
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal(21, decimal.Parse(result.Result));
        }


        [Fact]
        public async Task TestEvaluatorComplexNonExistingProductReturnsUndefined()
        {
            string expression = "SUM(NONEXISTINGMETRIC([ADA,XRP],LATEST))";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;

            //  todo extract string to const
            // todo extract preparation steps , services and empty db 

            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(NewMetricDto(sourceId, ValueCode))).Id;


            ProductDto prodDto = NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(valueId, productId, 10, Currency, ADAProductCode, ValueCode));

            prodDto = NewProductDto(InventoryId, XRPProductCode);
            productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(valueId, productId, 11, Currency, XRPProductCode, ValueCode));


            Evaluator instance = new Evaluator(_mediator, _expressionDbContext);
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal(  EvaluatorResult.EvaluatorResultType.undefined, result.Type);
            Assert.Equal(string.Empty, result.Result);
        }


        [Fact]
        public async Task TestEvaluator()
        {
            string expression = "VALUE([ADA]) > 100 ";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;

            //  todo extract string to const
            // todo extract preparation steps , services and empty db 

            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(NewMetricDto(sourceId, ValueCode))).Id;


            ProductDto prodDto = NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(valueId, productId, 10, Currency, ADAProductCode, ValueCode));

            prodDto = NewProductDto(InventoryId, XRPProductCode);
            productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetric(NewProdctMetricDto(valueId, productId, 11, Currency, XRPProductCode, ValueCode));


            Evaluator instance = new Evaluator(_mediator, _expressionDbContext);
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal("False", result.Result);
        }





        /// todo: move factories to dto's 

        private static ProductMetricDto NewProdctMetricDto(Guid metricId, Guid productId,
            int quantity, string currency, string productCode, string metricCode   )
        {
            return new ProductMetricDto(productId, metricId, quantity, DateTime.MinValue, currency, productCode, metricCode);
        }

        private static  ProductDto NewProductDto(Guid InventoryId, string productCode  )
        {
            return new ProductDto(Guid.NewGuid(),
                                         "",
                                         productCode,
                                            InventoryId,
                                         new List<ProductMetricDto>());
        }
        private static MetricDto NewMetricDto(Guid SourceId,string MetricCode )
        {
            return new MetricDto(Guid.NewGuid(), "", MetricCode, SourceId);
        }
    }
}
