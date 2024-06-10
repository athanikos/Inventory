using Inventory.Products.Contracts;
using dto = Inventory.Products.Contracts.Dto;
using Inventory.Products.Repositories;
using MediatR;

namespace Inventory.Products.Handlers
{
    public class AddProdcutMetricHandler : IRequestHandler<AddProductMetricCommand,dto.ProductMetricDto>
    {
        private IInventoryRepository _repository;
        
        public AddProdcutMetricHandler(IInventoryRepository repository) 
        {
            _repository = repository;   
        }    


        public async Task<dto.ProductMetricDto> Handle(AddProductMetricCommand request, CancellationToken cancellationToken)
        {
            var dto = new dto.ProductMetricDto()
            {
                EffectiveDate = DateTime.UtcNow,
                MetricId = request.MetricId,
                ProductId = request.ProductId,
                Value = request.Value
            };
            
           await  _repository.AddOrEditProductMetric(dto);
           return dto;
        }

        
    }
}
