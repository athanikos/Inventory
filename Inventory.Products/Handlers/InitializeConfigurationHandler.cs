using Inventory.Products.Contracts;
using Inventory.Products.Services;
using MediatR;

namespace Inventory.Products.Handlers
{
    public class InitializeConfigurationHandler(IInventoryService service) :
        IRequestHandler<InitializeConfigurationCommand, List<InitializeConfigurationResponse>>
    {
        public  async Task<List<InitializeConfigurationResponse>> Handle(InitializeConfigurationCommand request, CancellationToken cancellationToken)
        {
            return  await service.InitialConfigureAsync();
        }
    }


}
