using MediatR;
using Inventory.Products.Dto;
using Category.Products.Endpoints;
using Inventory.Products.Endpoints;
using Inventory.Products.Repositories;


namespace Inventory.Products.Handlers
{
    internal class DeleteCategoryHandler :
        IRequestHandler<DeleteCategoryCommand>
    {

        private readonly IInventoryRepository _categoryRepository;
         

        public DeleteCategoryHandler(IInventoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public async Task Handle(DeleteCategoryCommand request,
            CancellationToken cancellationToken)
        {
        
            await  _categoryRepository.DeleteCategoryAsync(new CategoryDto(request.Id, string.Empty, Guid.Empty));
        }
    }
}
