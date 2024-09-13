
namespace Transaction.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;
    using Microsoft.AspNetCore.Http;

    public class TemplatesDataPrepare :
        Endpoint<DataPrepareRequest>
    {
        private readonly IDataPrepareRepository _repo;

        public TemplatesDataPrepare(IDataPrepareRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Post("/TemplatesDataPrepare");
            AllowAnonymous();
            // to do claims this is per EntityId claim
            //  something like Admin_<EntityId>
        }

        public override async Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DataPrepareRequest edbr, CancellationToken ct)
        {

             _repo.RoomsPrepare();
             await   _repo.SaveChangesAsync();   
             return TypedResults.Ok();
        }
    }
  
    public record DataPrepareRequest(string Name)
    { }
}
