
namespace Transaction.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;
    using Microsoft.AspNetCore.Http;

    public class EditEntity :
        Endpoint<EditEntityRequest>
    {
        private readonly ITransactionRepository _repo;

        public  EditEntity(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Put("/entity");
            // to do claims this is per EntityId claim
            //  something like Admin_<EntityId>
        }

        public override async Task<Results<Ok<EntityDto>, NotFound, ProblemDetails>>
            HandleAsync(EditEntityRequest req,
                        CancellationToken ct)
        {

            var dto = await _repo.EditEntityAsync(
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
    public record EditEntityRequest(Guid EntityId, string Description, DateTime Created, ICollection<ValueDto> Values);

  
}
