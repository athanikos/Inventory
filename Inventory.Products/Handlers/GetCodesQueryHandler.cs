using Inventory.Products.Contracts;
using Inventory.Products.Repositories;
using MediatR;

namespace Inventory.Products.Handlers
{
    public  class CodesQueryHandler :
    IRequestHandler<CodesQuery, CodesResponse>
    {

        private readonly IInventoryRepository _repo;


        public CodesQueryHandler(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<CodesResponse> Handle(CodesQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new CodesResponse()
            {
                   ProductCodes =  _repo.GetDistinctProductCodes(request.InventoryId),
                   MetricCodes =   _repo.GetDistinctMetricCodes()   
            };
        
        }


    }
}
