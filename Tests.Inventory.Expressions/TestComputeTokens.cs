using MediatR;

namespace Tests.Inventory.Expressions
{
    public class EvaluatorTests
    {

        private readonly IMediator _mediator;


        public EvaluatorTests(IMediator mediator)
        {
            _mediator = mediator;   
        }

        [Fact]
        public void TestComputeTokens()
        {

            Assert.Equal(1, 1);

        }
    }
}
