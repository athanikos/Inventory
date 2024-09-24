using Inventory.Products.Contracts;
using Inventory.Products.Repositories;
using MediatR;
using Inventory.Products.Contracts.Dto;
using Serilog;

namespace Inventory.Products.Handlers
{

    public class CancellQuantityMetricHandler(IInventoryRepository repository) :
        IRequestHandler<CancelQuantityMetricCommand,
            List<QuantityMetricDto>>
    {
        public async Task<List<QuantityMetricDto>> Handle(CancelQuantityMetricCommand
                     request, CancellationToken cancellationToken)
        {
            try
            {
                await repository.CancellQuantityMetricsAsync(request.TransactionId);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return await repository.GetQuantityMetricsAsync(request.TransactionId);
        }
    }


}
