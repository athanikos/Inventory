
using Inventory.Expressions.Services;

namespace Inventory.Products.Endpoints
{
    using FastEndpoints;
    using global::Expressions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;

    public  class RunEvaluator
        : Endpoint <RunEvaluatorRequest>
    {

        private readonly IEvaluatorService _evaluatorService; 

        public RunEvaluator(IEvaluatorService evaluatorService)
        {
            _evaluatorService = evaluatorService; 
        }

        public override void Configure()
        {
            Post("/Evaluator");
            // to do claims this is per InventoryId claim
            //  something like Admin_<inventoryId>
        }

        public override  async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(RunEvaluatorRequest req,
                        CancellationToken ct)
        {
            _evaluatorService.DoScheduledWork();
            await Task.CompletedTask;
            return TypedResults.Ok();
        }


    }

    public record RunEvaluatorRequest(string Id );
}
