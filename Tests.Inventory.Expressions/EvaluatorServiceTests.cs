



using Inventory.Expressions;
using Inventory.Expressions.Repositories;
using Inventory.Expressions.Services;
using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;
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
    public class EvaluatorServiceTests : TestBed<TestFixture>
    {
        private const string ADAProductCode = "ADA";
        private const string XRPProductCode = "XRP";
        private const string Crypto = "CRYPTO";
        private const string Currency = "EUR";
        private const string SourceName = "Source";
        private const string ValueCode = "VALUE";
        private const string QuantityCode = "QUANTITY";
        private const string PriceCode = "PRICE";


        public EvaluatorServiceTests(ITestOutputHelper testOutputHelper, TestFixture fixture) :
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

            EvaluatorService instance = new (_mediator, _expressionsRepo);
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


            var mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;
           
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;


            var quantityId = output.QuantityId;
            var priceId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, PriceCode))).Id;
      

               await output.InventoryRepo.AddQuantityMetricAsync(new QuantityMetricDto(output.ProductId, value:5, DateTime.MinValue, output.TransactionId,300, false));


            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(priceId, output.ProductId, 5, Currency, 
                ADAProductCode, Constants.EmptyUnityOfMeasurementId));

            EvaluatorService instance = new (mediator, expressionsRepo);
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal(25, decimal.Parse(result.Result));
        }

        private static async Task<Tuple<Guid,Guid>> SetupInventoryAndSource(IInventoryRepository _repo)
        {
            var inventoryId = (await _repo.AddInventoryAsync(new InventoryDto(Guid.NewGuid(), Crypto,Crypto))).Id;
            var sourceId = (await _repo.AddSourceAsync(new SourceDto(Guid.NewGuid(),SourceName))).Id;
            return new Tuple<Guid, Guid>(inventoryId, sourceId);    
        }

        [Fact]
        public async Task TestEvaluatorComplexSingleProduct()
        {
            string expression = "SUM(VALUE([ADA],LATEST))";
            var mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;


            var _expressionDbContext = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            _repo.EmptyDb();

            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, ValueCode))).Id;
     
            
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 10, 
                Currency, ADAProductCode,Constants.EmptyUnityOfMeasurementId));
      

            EvaluatorService instance = new (mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId,expression);
            Assert.Equal(10, decimal.Parse(result.Result));
        }


        [Fact]
        public async Task TestEvaluatorComplexAllProduct()
        {
            string expression = "SUM(VALUE([ALL],LATEST))";

            var mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;

            repo.EmptyDb();
            var tuple = (await SetupInventoryAndSource(repo));
            var inventoryId = tuple.Item1;
            var sourceId = tuple.Item2;
            var valueId = (await repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, ValueCode))).Id;

            ProductDto prodDto = ProductDto.NewProductDto(inventoryId, ADAProductCode);
            var productId = (await repo.AddProductAsync(prodDto)).Id;
            await repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 10, Currency, ADAProductCode,  Constants.EmptyUnityOfMeasurementId));

            prodDto = ProductDto.NewProductDto(inventoryId, XRPProductCode);
            productId = (await repo.AddProductAsync(prodDto)).Id;
            await repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 100, Currency, XRPProductCode,  Constants.EmptyUnityOfMeasurementId));

            EvaluatorService instance = new EvaluatorService(mediator, expressionsRepo);
            var result = await instance.Execute(inventoryId,expression);
            Assert.Equal(110, decimal.Parse(result.Result));
        }

        [Fact]
        public async Task TestEvaluatorComplexTwoProducts()
        {
            string expression = "SUM(VALUE([ADA,XRP],LATEST))";

            var mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;


            _ = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;
            repo.EmptyDb();
            var tuple = (await SetupInventoryAndSource(repo));
            var inventoryId = tuple.Item1;
            var sourceId = tuple.Item2;

            var valueId = (await repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, ValueCode))).Id;


            ProductDto prodDto = ProductDto.NewProductDto(inventoryId,ADAProductCode);
            var productId = (await repo.AddProductAsync(prodDto)).Id;
            await repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 10, Currency, ADAProductCode,  Constants.EmptyUnityOfMeasurementId));

            prodDto = ProductDto.NewProductDto(inventoryId, XRPProductCode);
            productId = (await repo.AddProductAsync(prodDto)).Id;
            await repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 11, Currency, XRPProductCode,  Constants.EmptyUnityOfMeasurementId));


            EvaluatorService instance = new (mediator, expressionsRepo);
            var result = await instance.Execute(inventoryId, expression);
            Assert.Equal(21, decimal.Parse(result.Result));
        }


        [Fact]
        public async Task TestEvaluatorComplexNonExistingProductReturnsUndefined()
        {
            string expression = "SUM(NONEXISTINGMETRIC([ADA,XRP],LATEST))";


            var _mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var _expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;
            _repo.EmptyDb();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, ValueCode))).Id;


            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 10, Currency, ADAProductCode, 
                Constants.EmptyUnityOfMeasurementId));

            prodDto = ProductDto.NewProductDto(InventoryId, XRPProductCode);
            productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 11, Currency, XRPProductCode, 
                Constants.EmptyUnityOfMeasurementId));


            EvaluatorService instance = new (_mediator, _expressionsRepo);
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
            _repo.EmptyDb();
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
                                                                        ADAProductCode,
                                                                        Constants.Quantitycode,
                                                                        Constants.EmptyUnityOfMeasurementId
                                                                        ));

            prodDto = ProductDto.NewProductDto(InventoryId, XRPProductCode);
            productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 11, Currency, XRPProductCode,  Constants.EmptyUnityOfMeasurementId));


            EvaluatorService instance = new (_mediator, _expressionsRepo);
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
            _repo.EmptyDb();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var valueId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, PriceCode))).Id;


            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;

            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(valueId, productId, 2, 
                ADAProductCode, string.Empty, Constants.EmptyUnityOfMeasurementId));
    
            EvaluatorService instance = new EvaluatorService(_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId, expression);
            Assert.Equal("True", result.Result);
        }


        [Fact]
        public async Task TestEvaluatorPriceWhenItDoesNotExist()
        {
            string expression = "PRICE([ADA]) > 1 ";


            var mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            IInventoryRepository repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;

            repo.EmptyDb();
            var tuple = (await SetupInventoryAndSource(repo));
            var inventoryId = tuple.Item1;

            EvaluatorService instance = new (mediator, expressionsRepo);
            var result = await instance.Execute(inventoryId, expression);
            Assert.Equal(EvaluatorResult.EvaluatorResultType.undefined, result.Type);
        }


        [Fact]
        public async Task TestEvaluatorPriceIfGreaterThanSomefloat()
        {
            string expression = "PRICE([ADA]) > 0.35 ";


            var mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
            var repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            var expressionsRepo = _fixture.GetService<IExpressionRepository>(_testOutputHelper)!;

            //  todo extract string to const
            // todo extract preparation steps , services and empty db 

            repo.EmptyDb();
            var tuple = (await SetupInventoryAndSource(repo));
            var inventoryId = tuple.Item1;
            var sourceId = tuple.Item2;

            var priceId = (await repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, PriceCode))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(inventoryId, ADAProductCode);
            var productId = (await repo.AddProductAsync(prodDto)).Id;
            await repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(priceId, productId, 1, Currency, ADAProductCode,  Constants.EmptyUnityOfMeasurementId));



            EvaluatorService instance = new (mediator, expressionsRepo);
            var result = await instance.Execute(inventoryId, expression);
          
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

            _repo.EmptyDb();
            Guid InventoryId, sourceId;
            var tuple = (await SetupInventoryAndSource(_repo));
            InventoryId = tuple.Item1;
            sourceId = tuple.Item2;

            var priceId = (await _repo.AddMetricAsync(MetricDto.NewMetricDto(sourceId, PriceCode))).Id;
            ProductDto prodDto = ProductDto.NewProductDto(InventoryId, ADAProductCode);
            var productId = (await _repo.AddProductAsync(prodDto)).Id;
            await _repo.AddOrEditProductMetricAsync(ProductMetricDto.NewProductMetricDto(priceId, productId, 1, Currency, ADAProductCode,  Constants.EmptyUnityOfMeasurementId));



            EvaluatorService instance = new EvaluatorService(_mediator, _expressionsRepo);
            var result = await instance.Execute(InventoryId, expression);

            Assert.Equal(EvaluatorResult.EvaluatorResultType.boolean, result.Type);
            Assert.Equal("True", result.Result);


        }
    }
}
