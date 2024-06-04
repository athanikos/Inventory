using Inventory.Products.Dto;

namespace Inventory.Products.Repositories
{
    public interface ITransactionRepository
    {
        Task<TransactionDto> AddTransactionAsync(TransactionDto dto);
        bool CategoryFatherIdExists(Guid FatherId);
        Task<TransactionDto> EditTransactionAsync(TransactionDto c);
        Task DeleteTransactionAsync(TransactionDto c);

        Task<TransactionItemDto> AddTransactionItemAsync(TransactionItemDto ti);
        Task<TransactionItemDto> EditTransactionItemAsync(TransactionItemDto ti);
        Task DeleteTransactionItemAsync(TransactionItemDto ti);
    }
}