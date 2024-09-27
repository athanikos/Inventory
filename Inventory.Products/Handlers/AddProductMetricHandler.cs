using Inventory.Products.Contracts;
using dto = Inventory.Products.Contracts.Dto;
using Inventory.Products.Repositories;
using MediatR;
using Serilog;

namespace Inventory.Products.Handlers
{
    public class AddProductMetricHandler(
        IInventoryRepository
            repository) :
        IRequestHandler<AddProductMetricCommand, dto.ProductMetricDto>
    {
        public async Task<dto.ProductMetricDto> Handle(AddProductMetricCommand request,
            CancellationToken cancellationToken)
        {

            var dto = new dto.ProductMetricDto(request.ProductId,
                request.MetricId,request.Value,
                request.EffectiveDate,string.Empty,
                string.Empty, request.UnitOfMeasurementId
                             );
            try
            {
                await repository.AddOrEditProductMetricAsync(dto);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            return dto;
        }


    }
}
