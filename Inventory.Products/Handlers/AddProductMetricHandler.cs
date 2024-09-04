using Inventory.Products.Contracts;
using dto = Inventory.Products.Contracts.Dto;
using Inventory.Products.Repositories;
using MediatR;
using Serilog;

namespace Inventory.Products.Handlers
{
    public class AddProductMetricHandler :
        IRequestHandler<AddProductMetricCommand, dto.ProductMetricDto>
    {
        private IInventoryRepository _repository;

        public AddProductMetricHandler(IInventoryRepository
            repository)
        {
            _repository = repository;
        }


        public async Task<dto.ProductMetricDto> Handle(AddProductMetricCommand request,
            CancellationToken cancellationToken)
        {

            var dto = new dto.ProductMetricDto(request.ProductId,
                                                   request.MetricId,
                                                   request.Value,
                                                   request.EffectiveDate,
                                                   request.Currency,
                                                   string.Empty,
                                                   string.Empty
                             );
            try
            {
                await _repository.AddOrEditProductMetricAsync(dto);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            return dto;
        }


    }
}
