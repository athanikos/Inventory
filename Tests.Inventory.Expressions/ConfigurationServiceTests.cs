using Inventory.Defaults.Contracts;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;


namespace Tests.Inventory
{
    [Collection("Our Test Collection #1")]
    public class ConfigurationServiceTests(ITestOutputHelper testOutputHelper,  
        TestFixture fixture)
        : TestBed<TestFixture>(testOutputHelper, fixture)
    {
        [Fact]
        public async  Task TestOnInitializeGetValueShouldReturnGuiid()
        {
            var output = await TestSetup.Setup(_testOutputHelper, this._fixture);
            var config =  await output.InventoryService.InitialConfigureAsync();

            await output.ConfigurationService.SaveAsync(config);
            
            
             var res =   await output.ConfigurationService.GetValueAsync(ConfigurationType.DefaultSource);
             Assert.Equal("SELF", res.Value);
             res =   await output.ConfigurationService.GetValueAsync(ConfigurationType.DefaultCurrency);
             Assert.Equal("EURO", res.Value);
             res =   await output.ConfigurationService.GetValueAsync(ConfigurationType.EmptyUnitOfMeasurement);
             Assert.Equal("EMPTY", res.Value);
             
        }

        
    }
}
