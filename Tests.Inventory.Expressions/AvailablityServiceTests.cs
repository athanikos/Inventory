using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;


namespace Tests.Inventory
{
    [Collection("Our Test Collection #1")]
    public class AvailabilityServiceTests(ITestOutputHelper testOutputHelper,  
        TestFixture fixture)
        : TestBed<TestFixture>(testOutputHelper, fixture)
    {
        [Fact]
        public void TestOnUnavailabilityShouldReturnEmptyForProduct()
        {
          Assert.Equal(1,0);
        }

        [Fact]
        public void TestOnAvailableShouldReturnOneIntervalPerRoom()
        {
            Assert.Equal(1,0);
        }

        
        [Fact]
        public void TestOnAvailabilityShouldReturnMultipleIntervalsForInventory()
        {
            Assert.Equal(1,0);
        }
        
    }
}
