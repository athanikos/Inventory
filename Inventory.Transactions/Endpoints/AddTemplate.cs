
namespace Transaction.Transactions.Endpoints
{
    using FastEndpoints;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;
    using Microsoft.AspNetCore.Http;
    using System.Text.Json.Serialization;

    public class AddTemplate :
        Endpoint<AddTemplateRequest>
    {
        private readonly ITransactionRepository _repo;

        public AddTemplate(ITransactionRepository repo)
        {
            _repo = repo;
        }

        public override void Configure()
        {
            Post("/template");
            AllowAnonymous();
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TemplateDto>, NotFound, ProblemDetails>>
            HandleAsync(AddTemplateRequest req,
                        CancellationToken ct)
        {
            var dto = await _repo.AddTemplateAsync(
                new TemplateDto(Guid.Empty, req.Name, req.Type, DateTime.UtcNow, req.Sections));
                
            return TypedResults.Ok(dto);
        }
    }

    public class AddTemplateRequest
    {
        public AddTemplateRequest(Guid Id, string Name, TemplateType Type, DateTime Created, ICollection<SectionDto> Sections)
        {
            this.Id = Id;
            this.Name = Name;
            this.Type = Type;
            this.Created = Created;
            this.Sections = Sections;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TemplateType Type { get; set; }
        public DateTime Created { get; set; }
        public ICollection<SectionDto> Sections { get; set; }

    }
}
