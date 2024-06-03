using MediatR;
using Inventory.Products.Dto;
using Category.Products.Endpoints;
using Inventory.Products.Repositories;

namespace Inventory.Products.Handlers
{
    internal class AddCategoryHandler :
        IRequestHandler<AddCategoryCommand, CategoryDto>
    {

        private readonly ICategoryRepository _repository;

        public AddCategoryHandler(ICategoryRepository repo)
        {
            _repository = repo;
        }

        public async Task<CategoryDto> Handle(AddCategoryCommand request,
            CancellationToken cancellationToken)
        {
           return await _repository.AddCategoryAsync(new CategoryDto(Guid.NewGuid(), request.Description, request.FatherId));
        } 
    }
}
