using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Tests.Inventory
{


    /// <summary>
    /// https://github.com/Umplify/xunit-dependency-injection/blob/main/examples/Xunit.Microsoft.DependencyInjection.ExampleTests/CalculatorTests.cs
    /// </summary>
    [Collection("Our Test Collection #1")]
    public class QuantityMetricTests(ITestOutputHelper testOutputHelper, TestFixture fixture) : TestBed<TestFixture>(testOutputHelper, fixture)
    {

        [Fact]
        public async Task TestUniqueConstraintOnMinutesDifferenceTwoRecordsAreInserted()
        {

            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);


            DateTime dateWithMinutes = new(2020, 1, 1, 1, 1, 0);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1, 
                dateWithMinutes, output. TransactionId, 1, false, ModificationType.Buy);
            _ = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);
            DateTime dateWithMinutes2 = new(2020, 1, 1, 1, 13, 0);

            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1, 
                dateWithMinutes2, output.TransactionId, 1, false, ModificationType.Buy);
            await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto2);

            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Equal(2, qms.Count);
        }

        [Fact]
        public async Task TestUniqueConstraintSameRecordIsNotInsertedTwice()
        {

            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);

            DateTime dateWithMinutes = new(2020, 1, 1, 1, 13, 0);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1, 
                dateWithMinutes, output.TransactionId, 1, false, ModificationType.Buy);
            _ = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);
            DateTime dateWithMinutes2 = new(2020, 1, 1, 1, 13, 0);

            var quantityMetricDto2 = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1,  
                dateWithMinutes2, output.TransactionId, 1, false, ModificationType.Buy);
            try
            {
                await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto2);
            }
            catch (Exception) // throws exception on unique constraint 
            {
            }
            var qms = await output.InventoryRepo.GetQuantityMetricsAsync();
            Assert.Single(qms);


            // workaround fix to clear db records, records  are kept in next test and cause to fail 
            await TestSetup.ClearDb(_testOutputHelper, this._fixture);


        }

        [Fact]
        public async Task TestTwoIdenticalRecordsAddedToContextNoneIsSaved()
        {

            var output = await TestSetup.Setup(_testOutputHelper, _fixture);
       
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 1,
                new DateTime(1990,1,1,1,1,1), output. TransactionId, 1, false,ModificationType.Buy);

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

        
    }

}