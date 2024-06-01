using FastEndpoints;
using FluentValidation;


namespace Inventory.Products.Endpoints.Validation
{
    public  class ProductValidator : 
        Validator<AddProductRequest>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required!");

             RuleFor(x => x.InventoryId)
            .NotEmpty()
            .WithMessage("Inventory is required"); 

        }


    }
}
