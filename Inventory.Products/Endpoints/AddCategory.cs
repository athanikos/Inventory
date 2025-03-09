using FastEndpoints;
using Inventory.Products.Dto;
using Inventory.Products.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inventory.Products.Endpoints
{

    public class AddCategory(IMediator mediator, IInventoryService service) : Endpoint<AddCategoryRequest>
    {
        private readonly IMediator _mediator = mediator;

        public override void Configure()
        {
            Post("/categories");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok<CategoryDto>, NotFound, ProblemDetails>> HandleAsync(AddCategoryRequest req, CancellationToken ct)
        {
            try
            {
                return TypedResults.Ok(await service.AddCategoryAsync(AddCategoryRequestExtensions.ToDto(req)));
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                ThrowIfAnyErrors(); // If there are errors, execution shouldn't go beyond this point
                return new ProblemDetails(ValidationFailures);
            }
        }
    }


    public abstract record AddCategoryRequest(Guid FatherId, string Description);

    public static class AddCategoryRequestExtensions
    {
        public static CategoryDto ToDto(this AddCategoryRequest req)
        {
            return new CategoryDto(Guid.NewGuid(), req.Description, req.FatherId);
        }
    }   



}
