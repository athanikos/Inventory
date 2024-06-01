using FastEndpoints;
using FluentValidation;


namespace Inventory.Products.Endpoints.Validation
{
    public  class InventoryValidator : 
        Validator<AddInventoryRequest>
    {
        public InventoryValidator()
        {
            RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required!");      
        }


    }
}
