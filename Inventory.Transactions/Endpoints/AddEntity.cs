
namespace Transaction.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;
    using Microsoft.AspNetCore.Http;

    public class AddEntity :
        Endpoint<AddEntityRequest>
    {
        private readonly ITransactionRepository _repo;

        public  AddEntity(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Post("/entity");
            // to do claims this is per EntityId claim
            //  something like Admin_<EntityId>
        }

        public override async Task<Results<Ok<EntityDto>, NotFound, ProblemDetails>>
            HandleAsync(AddEntityRequest req,
                        CancellationToken ct)
        {

            var dto = await _repo.AddEntityAsync(
                new EntityDto()
                {
                    Id = req.EntityId,
                    Created = req.Created,
                    Description = req.Description,
                    Values = req.Values
                });

            return TypedResults.Ok<EntityDto>(dto);
        }
    }
    public record AddEntityRequest(Guid EntityId, string Description, DateTime Created, ICollection<ValueDto> Values);

  
}
