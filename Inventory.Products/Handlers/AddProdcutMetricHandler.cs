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
            var dto = new dto.ProductMetricDto(request.ProductId,
                                               request.MetricId,
                                               request.Value,
                                               request.EffectiveDate,
                                               request.Currency,
                                               string.Empty,
                                               string.Empty
                         );



            
           await  _repository.AddOrEditProductMetric(dto);
           return dto;
        }

        
    }
}
