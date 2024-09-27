
namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using Inventory.Products.Contracts.Dto;
    using Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public class ModifyQuantityMetric(IModifyQuantityService service) :
        Endpoint<ModifyQuantityMetricListRequest>
    {
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


            await  service.ModifyQuantityMetricsAsync( items ); 
            return TypedResults.Ok();
        }  
    }

    public abstract record ModifyQuantityMetricListRequest(
        List<ModifyQuantityMetricRequest> Items 
        );


    public abstract record ModifyQuantityMetricRequest(Guid ProductId, 
         decimal Diff , DateTime EffectiveDate );



}
