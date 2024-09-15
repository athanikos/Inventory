using Inventory.Expressions;
using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;
using Inventory.Transactions.Dto;
using Inventory.Transactions.Repositories;
using MediatR;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Tests.Inventory.Expressions
{


    /// <summary>
    /// https://github.com/Umplify/xunit-dependency-injection/blob/main/examples/Xunit.Microsoft.DependencyInjection.ExampleTests/CalculatorTests.cs
    /// </summary>
    [Collection("Our Test Collection #1")]
    public class QuantityMetricTests(ITestOutputHelper testOutputHelper, TestFixture fixture) : TestBed<TestFixture>(testOutputHelper, fixture)
    {
 
        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestOnAddQuantityMetricItShouldAddtoProductMetricAsWell()
        {
          
        
            var output = await TestSetup.Setup(_testOutputHelper,this._fixture);

          
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1, DateTime.MinValue, output.TransactionId, 1, false);
            var qm = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);
            var pm = await output.InventoryRepo.GetProductMetricAsync(output.ProductId, DateTime.MinValue);
            Assert.NotNull(pm);
            Assert.NotNull(qm);
        }

  
        [Fact]
        public async Task TestUniqueConstraintOnMinutesDifferenceTwoRecordsAreInserted()
        {

            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);


            DateTime dateWithMinutes = new(2020, 1, 1, 1, 1, 0);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1,  dateWithMinutes, output. TransactionId, 1, false);
            _ = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);
            DateTime dateWithMinutes2 = new(2020, 1, 1, 1, 12, 0);

            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1,  dateWithMinutes2, output.TransactionId, 1, false);
            await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto2);

            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Equal(2, qms.Count);
        }

        [Fact]
        public async Task TestUniqueConstraintSameRecordIsNotInsertedTwice()
        {

            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);

            DateTime dateWithMinutes = new(2020, 1, 1, 1, 12, 0);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1,  dateWithMinutes, output.TransactionId, 1, false);
            _ = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);
            DateTime dateWithMinutes2 = new(2020, 1, 1, 1, 12, 0);

            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1,  dateWithMinutes2, output.TransactionId, 1, false);
            try
            {
                await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto2);
            }
            catch (Exception) // throws exeeption on unique constraint 
            {
            }
            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Single(qms);
        }

        [Fact]
        public async Task TestTwoIdenticalRecordsAddedToContextNoneIsSaved()
        {

            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);


            var templateId = await output.TransactionRepo.RoomsPrepareAsync();
            var TransactionId = (await output.TransactionRepo.AddTransactionAsync(new TransactionDto(Guid.NewGuid(), "", DateTime.Now, templateId, null))).Id;
            _ = _fixture.GetService<ITransactionRepository>(_testOutputHelper)!;


            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1,  new DateTime(2000,1,1,1,1,1), TransactionId, 1, false);


            try 
            {
                output.InventoryRepo.AddQuantityMetric(quantityMetricDto);
                output.InventoryRepo.AddQuantityMetric(quantityMetricDto); // fails on context add not even on saves 
                await output.InventoryRepo.SaveChangesAsync(); 
            }
            catch (Exception) { }

            var qms = await  output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Empty(qms);
        }

        [Fact]
        public async Task TestMultipleIntervalsAreNotSavedWhenAtleastOneIsNotAvailable()
        {
             var _repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;
            _ = _fixture.GetService<ExpressionsDbContext>(_testOutputHelper)!;

            //todo implement  insert one qa for some interval 
            // save 
            // then attempt to add 3 intervals with 1 overalapping with then one above 



            await _repo.GetQuantityMetricsAsync();
        }
    }

}