
using System.Text.Json.Serialization;
using FastEndpoints;
using Inventory.Transactions.Dto;
using Inventory.Transactions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inventory.Transactions.Endpoints
{
    public class AddTemplate(ITransactionService service) :
        Endpoint<AddTemplateRequest>
    {
        public override void Configure()
        {
            Post("/templates");
            AllowAnonymous();
            // to do claims this is per TransactionId claim
            //  something like Admin_<TransactionId>
        }

        public override async Task<Results<Ok<TemplateDto>, NotFound, ProblemDetails>>
            HandleAsync(AddTemplateRequest req,
                        CancellationToken ct)
        {
            var dto = await service.AddTemplateAsync
                (
                new TemplateDto(Guid.Empty,
                req.Name, req.Type,
                DateTime.UtcNow,
                req.Sections)
                );

            return TypedResults.Ok(dto);
        }
    }

    public class AddTemplateRequest(
        Guid id,
        string name,
        TemplateType type,
        DateTime created,
        ICollection<SectionDto> sections)
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TemplateType Type { get; set; } = type;

        public DateTime Created { get; set; } = created;
        public ICollection<SectionDto> Sections { get; set; } = sections;
    }
}
