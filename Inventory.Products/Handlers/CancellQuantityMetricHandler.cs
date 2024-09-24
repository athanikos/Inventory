using Inventory.Products.Contracts;
using Inventory.Products.Repositories;
using MediatR;
using Inventory.Products.Contracts.Dto;
using Serilog;

namespace Inventory.Products.Handlers
{

    public class CancellQuantityMetricHandler :
    IRequestHandler<CancellQuantityMetricCommand,
        List<QuantityMetricDto>>
    {
        private IInventoryRepository _repository;

        public CancellQuantityMetricHandler(
            IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<QuantityMetricDto>> Handle(CancellQuantityMetricCommand
                     request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.CancellQuantityMetricsAsync(request.TransactionId);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return await _repository.GetQuantityMetricsAsync(request.TransactionId);
        }
    }


}
