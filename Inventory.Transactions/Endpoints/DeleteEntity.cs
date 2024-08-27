
namespace Transaction.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;
    using Microsoft.AspNetCore.Http;

    public class DeleteEntity :
        Endpoint<DeleteEntityRequest>
    {
        private readonly ITransactionRepository _repo;

        public  DeleteEntity(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Delete("/entity");
            // to do claims this is per EntityId claim
            //  something like Admin_<EntityId>
        }

        public override async  Task<Results<Ok, NotFound, ProblemDetails>>
            HandleAsync(DeleteEntityRequest req,
                        CancellationToken ct)
        {
            await _repo.DeleteEntityAsync(
                new EntityDto()
                {
                    Id = req.EntityId,
                    Created = DateTime.UtcNow,
                    Description = string.Empty,
                    Values = new List<ValueDto>()
                });

            return TypedResults.Ok();
        }
    }
    public record DeleteEntityRequest(Guid EntityId);

  
}
