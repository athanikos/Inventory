using Inventory.Products.Contracts;
using Inventory.Products.Repositories;
using MediatR;

namespace Inventory.Products.Handlers
{
    public class CodesQueryHandler(IInventoryRepository repo) :
        IRequestHandler<CodesQuery, CodesResponse>
    {
        //todo pass service


        public async Task<CodesResponse> Handle(CodesQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new CodesResponse()
            {
                   ProductCodes =  repo.GetDistinctProductCodes(request.InventoryId),
                   MetricCodes =   repo.GetDistinctMetricCodes()   
            };
        
        }


    }
}
