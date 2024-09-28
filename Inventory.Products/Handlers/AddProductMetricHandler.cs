using Inventory.Products.Contracts;
using DTO = Inventory.Products.Contracts.Dto;
using Inventory.Products.Repositories;
using MediatR;
using Serilog;

namespace Inventory.Products.Handlers
{
    public class AddProductMetricHandler(
        IInventoryRepository
            repository) :
        IRequestHandler<AddProductMetricCommand, DTO.ProductMetricDto>
    {
        public async Task<DTO.ProductMetricDto> Handle(AddProductMetricCommand request,
            CancellationToken cancellationToken)
        {

            var dto = new DTO.ProductMetricDto(request.ProductId,
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
