using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;


namespace Tests.Inventory
{
    [Collection("Our Test Collection #1")]
    public class TransactionServiceTests(ITestOutputHelper testOutputHelper,  
        TestFixture fixture)
        : TestBed<TestFixture>(testOutputHelper, fixture)
    {
        [Fact]
        public void TestOnCancellationAllRecordsAreCancelledAndTransactionStatusBecomesCanceled()
        {
            Assert.Equal(1,0);
        }

        [Fact]
        public  void TestOnTemplateCreatedOnNewTransactionIsReturnedWithAllFieldsWithDefaultValues()
        {
            Assert.Equal(1,0);
        }

        [Fact]
        public  void TestOnEditTransaction()
        {
            Assert.Equal(1,0);
        }
    }
}
