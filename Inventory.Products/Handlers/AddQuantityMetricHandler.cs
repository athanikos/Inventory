using Inventory.Products.Contracts;
using Inventory.Products.Repositories;
using MediatR;
using Serilog;
using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Handlers
{

    public class AddQuantityMetricHandler(
        IInventoryRepository
            repository) :
        IRequestHandler<AddQuantityMetricCommand, QuantityMetricDto>
    {
        public async Task<QuantityMetricDto> Handle(AddQuantityMetricCommand request,
                                                    CancellationToken cancellationToken)
        {
            var dto = new QuantityMetricDto(request.ProductId,
                                                request.Value,
                                                request.EffectiveDate,
                                                request.TransactionId,
                                                request.Diff,
                                                request.IsCancelled
                                               );
            try
            {
                await repository.AddQuantityMetricAsync(dto);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return dto;
        }
    }


}
