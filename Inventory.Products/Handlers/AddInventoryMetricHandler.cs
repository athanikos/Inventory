using Inventory.Products.Contracts;
using Inventory.Products.Contracts.Dto;
using Inventory.Products.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Products.Handlers
{
    internal class AddInventoryMetricHandler
    {
        private IInventoryRepository _repository;

        public AddInventoryMetricHandler(IInventoryRepository repository)
        {
            _repository = repository;
        }


        public async Task<InventoryMetricDto> Handle(AddProductMetricCommand request, CancellationToken cancellationToken)
        {
            var dto = new InventoryMetricDto(request.ProductId,
                                               request.MetricId,
                                               request.Value,
                                               request.EffectiveDate,
                                               request.Currency,
                                               string.Empty,
                                               string.Empty
                         );




            await _repository.AddOrEditInventoryMetric(dto);
            return dto;
        }

    }
}
