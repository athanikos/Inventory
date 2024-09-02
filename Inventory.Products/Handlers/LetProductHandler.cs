using MediatR;

namespace Inventory.Products.Handlers
{
    public class LetProductHandler
    : IRequestHandler<LetProductCommand, LetProductDto>
    {
        public Task<LetProductDto> Handle(LetProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
