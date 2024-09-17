using Expressions;
using Inventory.Expressions;
using Inventory.Expressions.Repositories;
using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Entities;
using Inventory.Products.Repositories;
using Inventory.Transactions.Repositories;
using MediatR;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Tests.Inventory
{


    /// <summary>
    /// https://github.com/Umplify/xunit-dependency-injection/blob/main/examples/Xunit.Microsoft.DependencyInjection.ExampleTests/CalculatorTests.cs
    /// </summary>
    /// 
    [Collection("Our Test Collection #1")]
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

            var output = await TestSetup.Setup(_testOutputHelper, _fixture,"ADA");
            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;

            var tuple = (await SetupInventoryAndSource(output.InventoryRepo));
            var  InventoryId = tuple.Item1;
            var sourceId = tuple.Item2;

            await output.InventoryRepo.AddQuantityMetricAsync(new QuantityMetricDto(output.ProductId, 1, DateTime.MinValue, output.TransactionId, 0, false));

            Evaluator instance = new (_mediator, _expressionsRepo);
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
            string expression = "QUANTITY([ADA]) * PRICE([ADA])";
            var output = await TestSetup.Setup(_testOutputHelper, _fixture, "ADA");


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;
           
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;


            var quantityId = output.QuantityId;
            var priceId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, PriceCode))).Id;
      

               await output.InventoryRepo.AddQuantityMetricAsync(new QuantityMetricDto(output.ProductId, value:5, DateTime.MinValue, output.TransactionId,300, false));


            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(priceId, output.ProductId, 5, Currency, 
                ADAProductCode, PriceCode));

            Evaluator instance = new (_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal(25, decimal.Parse(result.Result));
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
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;


            var _expressionDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            _repo.EmptyDB();

            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, ValueCode))).Id;
     
            
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 10, Currency, ADAProductCode,ValueCode));
      

            Evaluator instance = new (_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId,expression);
            Assert.Equal(10, decimal.Parse(result.Result));
        }


        [Fact]
        public async Task TestEvaluatorComplexALLProduct()
        {
            string expression = "SUM(VALUE([ALL],LATEST))";

            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;

            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;
            var valueId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, ValueCode))).Id;

            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 10, Currency, ADAProductCode, ValueCode));

            prodDto = ProductDto.NewProductDto(InventoryId, XRPProductCode);
            productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 100, Currency, XRPProductCode, ValueCode));

            Evaluator instance = new Evaluator(_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId,expression);
            Assert.Equal(110, decimal.Parse(result.Result));
        }

        [Fact]
        public async Task TestEvaluatorComplexTwoProducts()
        {
            string expression = "SUM(VALUE([ADA,XRP],LATEST))";

            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;


            var _expressionDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, ValueCode))).Id;


            ProductDto prodDto = ProductDto.NewProductDto(InventoryId,ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 10, Currency, ADAProductCode, ValueCode));

            prodDto = ProductDto.NewProductDto(InventoryId, XRPProductCode);
            productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 11, Currency, XRPProductCode, ValueCode));


            Evaluator instance = new (_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal(21, decimal.Parse(result.Result));
        }


        [Fact]
        public async Task TestEvaluatorComplexNonExistingProductReturnsUndefined()
        {
            string expression = "SUM(NONEXISTINGMETRIC([ADA,XRP],LATEST))";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;
            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, ValueCode))).Id;


            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 10, Currency, ADAProductCode, ValueCode));

            prodDto = ProductDto.NewProductDto(InventoryId, XRPProductCode);
            productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 11, Currency, XRPProductCode, ValueCode));


            Evaluator instance = new (_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal(  EvaluatorResult.EvaluatorResultType.undefined, result.Type);
            Assert.Equal(string.Empty, result.Result);
        }


        [Fact]
        public async Task TestEvaluatorValueGreaterThan100()
        {
            string expression = "VALUE([ADA]) > 100 ";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;
            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, ValueCode))).Id;


            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(
                                                                        valueId, 
                                                                        productId, 
                                                                        10, 
                                                                        Currency, 
                                                                        ADAProductCode,
                                                                        ValueCode));

            prodDto = ProductDto.NewProductDto(InventoryId, XRPProductCode);
            productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 11, Currency, XRPProductCode, ValueCode));


            Evaluator instance = new (_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal("False", result.Result);
        }

        [Fact]
        public async Task TestEvaluatorPriceGreaterThan1()
        {
            string expression = "PRICE([ADA]) > 1 ";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;
            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, PriceCode))).Id;


            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;

            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 2, Currency, ADAProductCode, PriceCode));
    
            Evaluator instance = new Evaluator(_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal("True", result.Result);
        }


        [Fact]
        public async Task TestEvaluatorPriceWhenItDoesNotExist()
        {
            string expression = "PRICE([ADA]) > 1 ";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;

            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            Evaluator instance = new (_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal(EvaluatorResult.EvaluatorResultType.undefined, result.Type);
        }


        [Fact]
        public async Task TestEvaluatorPriceIfGreaterThanSomefloat()
        {
            string expression = "PRICE([ADA]) > 0.35 ";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;

            //  todo extract string to const
            // todo extract preparation steps , services and empty db 

            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var priceId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, PriceCode))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(priceId, productId, 1, Currency, ADAProductCode, PriceCode));



            Evaluator instance = new (_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId, expression);
          
            Assert.Equal(EvaluatorResult.EvaluatorResultType.boolean, result.Type);
            Assert.Equal("True", result.Result);


        }


        [Fact]
        public async Task TestEvaluatorPriceIfGreaterThan070()
        {
            string expression = "PRICE([ADA]) > 0.70 ";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;

            //  todo extract string to const
            // todo extract preparation steps , services and empty db 

            _repo.EmptyDB();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var priceId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, PriceCode))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(priceId, productId, 1, Currency, ADAProductCode, PriceCode));



            Evaluator instance = new Evaluator(_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId, expression);

            Assert.Equal(EvaluatorResult.EvaluatorResultType.boolean, result.Type);
            Assert.Equal("True", result.Result);


        }
    }
}
