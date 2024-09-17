using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;


namespace Tests.Inventory
{
    [Collection("Our Test Collection #1")]
    public class CancellationQuantityMetricTests : TestBed<TestFixture>
    {

        public CancellationQuantityMetricTests(ITestOutputHelper testOutputHelper, TestFixture fixture) :
                          base(testOutputHelper, fixture)
        { }


        [Fact]
        public async Task TestOnCancellationAllRecordsAreCancelled()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);

            var firstDate = new DateTime(2021, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 10, firstDate, output.TransactionId, 1, false, ModificationType.Buy);
            var qm = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);

            await output.ModifyQuantityService.CancelQuantityMetricsAsync(
                new List<ModifyQuantityDto>() {
                  new ModifyQuantityDto()
                  {
                      ProductId = quantityMetricDto.ProductId,
                      Diff = 8,
                      EffectiveFrom = quantityMetricDto.EffectiveDate,
                      EffectiveTo = quantityMetricDto.EffectiveDate,
                      ModificationType = ModificationType.Sell
                  }});

            var qms = (await output.InventoryRepo.GetQuantityMetricsAsync()).OrderByDescending(p => p.EffectiveDate).ToList();
            Assert.Single(qms);
            Assert.True(qms[0].IsCancelled);
        }

        [Fact]
        public async Task TestOnCancellationAllRecordsAreCancelledWithNoPrevious()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);

            var firstDate = new DateTime(2021, 1, 1, 1, 1, 1);
            var quantityMetricDto = QuantityMetricDto.NewQuantityMetricDto(output.ProductId, 10, firstDate, output.TransactionId, 1, false, ModificationType.Buy);
            var qm = await output.InventoryRepo.AddQuantityMetricAsync(quantityMetricDto);

            await output.ModifyQuantityService.CancelQuantityMetricsAsync(
                new List<ModifyQuantityDto>() {
                  new ModifyQuantityDto()
                  {
                      ProductId = quantityMetricDto.ProductId,
                      Diff = 8,
                      EffectiveFrom = quantityMetricDto.EffectiveDate,
                      EffectiveTo = quantityMetricDto.EffectiveDate,
                      ModificationType = ModificationType.Sell
                  }});

            var qms = (await output.InventoryRepo.GetQuantityMetricsAsync()).OrderByDescending(p => p.EffectiveDate).ToList();
            Assert.Single(qms);
            Assert.True(qms[0].IsCancelled);
        }

    }
}
