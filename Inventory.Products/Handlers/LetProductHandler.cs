using MediatR;

namespace Inventory.Products.Handlers
{
    public class LetProductHandler
    : IRequestHandler<LetProductCommand, ModifyQuantityDto>
    {
        public Task<ModifyQuantityDto> Handle(LetProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
