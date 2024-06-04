using Inventory.Products.Endpoints;
using MediatR;
using Inventory.Products.Dto;
using Inventory.Products.Repositories;


namespace Inventory.Products.Handlers
{
    internal class EditCategoryHandler :
        IRequestHandler<EditCategoryCommand, CategoryDto>
    {
        private readonly IInventoryRepository _categoryRepository;

        public EditCategoryHandler(IInventoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> Handle(EditCategoryCommand request,
            CancellationToken cancellationToken)
        {
             return await  _categoryRepository.EditCategoryAsync(new CategoryDto(request.Id, request.Description, request.FatherId));
        }
    }

}
