

using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;

namespace Category.Products.Endpoints
{
    using FastEndpoints;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using System.Threading;
    using System.Threading.Tasks;
    using Inventory.Products.Dto;
    using System;
    using Inventory.Products.Repositories;

    [RequiredScope("products.read")]

    public class AddCategory :
        Endpoint<AddCategoryRequest>
    {
        private readonly IMediator _mediator;
        private readonly IInventoryRepository _repository;

        public  AddCategory(IMediator mediator, IInventoryRepository categoryRepository)
        {
            _mediator = mediator;
            _repository = categoryRepository;    
        }

        public override void Configure()
        {
            
            Post("/Category");
            // to do claims this is per CategoryId claim
            //  something like Admin_<CategoryId>
        }

        public override async Task<Results<Ok<CategoryDto>, NotFound, ProblemDetails>>
            HandleAsync(AddCategoryRequest req,
                        CancellationToken ct)
        {

            if (await _repository.CategoryIdExistsAsync(req.FatherId))
            {
                var categoryDto = await _repository.AddCategoryAsync(new CategoryDto(Guid.NewGuid(), req.Description, req.FatherId));
                return TypedResults.Ok(categoryDto);
            }
            else
            {
                AddError("FatherId does not exist ");
                ThrowIfAnyErrors(); // If there are errors, execution shouldn't go beyond this point
                return new FastEndpoints.ProblemDetails(ValidationFailures);

            }
        }
    }


    public abstract record AddCategoryRequest(Guid FatherId, string Description);

 
  
}
