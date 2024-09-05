
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Contracts.Dto;
    using Inventory.Products.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class ModifyQuantityMetric :
        Endpoint<ModifyQuantityMetricListRequest>
    {
        private readonly IInventoryRepository _repo;

        public ModifyQuantityMetric(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Post("/quantity");
            AllowAnonymous();//todo remove 
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(ModifyQuantityMetricListRequest req,
                        CancellationToken ct)
        {


             var items =  req.Items.Select(       i => 
                                                  new ModifyQuantityDto()
                                                  {
                                                                Diff =i.Diff,
                                                                EffectiveFrom = i.EffectiveDate,
                                                                EffectiveTo = i.EffectiveDate,  
                                                                ProductId = i.ProductId
                                                  }
                                           )
                                           .ToList();


            await _repo.ModifyQuantityMetrics(items);
            return TypedResults.Ok();
        }  
    }

    public record ModifyQuantityMetricListRequest(
        List<ModifyQuantityMetricRequest> Items 
        );


    public record ModifyQuantityMetricRequest(Guid ProductId, 
         decimal Diff , DateTime EffectiveDate );



}
