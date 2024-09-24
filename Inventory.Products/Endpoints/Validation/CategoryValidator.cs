using Category.Products.Endpoints;
using FastEndpoints;
using FluentValidation;


namespace Inventory.Products.Endpoints.Validation
{
    public  class CategoryValidator : 
        Validator<AddCategoryRequest>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required!");

             RuleFor(x => x.FatherId)
            .NotEmpty()
            .WithMessage("FatherId is required");

            
        }


    }
}
