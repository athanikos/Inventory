using FastEndpoints;
using Inventory.Expressions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inventory.Expressions.Endpoints
{
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
