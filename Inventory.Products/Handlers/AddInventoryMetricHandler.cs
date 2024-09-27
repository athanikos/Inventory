using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Repositories;
using Serilog;

namespace Inventory.Products.Handlers
{
    internal class AddInventoryMetricHandler(IInventoryRepository repository)
    {
        public async Task<InventoryMetricDto> Handle(AddInventoryMetricCommand request, CancellationToken cancellationToken)
        {
            var dto = new InventoryMetricDto( request.InventoryId,
                                              request.MetricId,
                                           
                                              request.Value,
                                              request.EffectiveDate,
                                              string.Empty,
                                              string.Empty
                         );



            try
            {
                await repository.AddOrEditInventoryMetric(dto);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }


            return dto;
        }

    }
}
