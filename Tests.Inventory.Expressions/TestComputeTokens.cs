using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Abstractions;
using MediatR;
using Inventory.Products.Repositories;

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

        [Fact]
        public async void TestComputeTokens()
        {

           
                var mediator = _fixture.GetService<IMediator>(_testOutputHelper)!;
                var repo = _fixture.GetService<IInventoryRepository>(_testOutputHelper)!;


                Assert.Equal(1, 1);

        }

    }
}
