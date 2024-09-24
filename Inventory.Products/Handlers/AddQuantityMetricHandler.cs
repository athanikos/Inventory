using Inventory.Products.Contracts;
using Inventory.Products.Repositories;
using MediatR;
using Serilog;
using Inventory.Products.Contracts.Dto;

namespace Inventory.Products.Handlers
{

    public class AddQuantityMetricHandler :
    IRequestHandler<AddQuantityMetricCommand, QuantityMetricDto>
    {
        private IInventoryRepository _repository;

        public AddQuantityMetricHandler(IInventoryRepository
            repository)
        {
            _repository = repository;
        }

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
                await _repository.AddQuantityMetricAsync(dto);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return dto;
        }
    }


}
