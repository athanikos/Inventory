using Inventory.Transactions.Dto;

namespace Inventory.Transactions.Repositories
{
    public interface ITransactionRepository
    {
        Task<TransactionDto> AddTransactionAsync(TransactionDto dto);
        Task<TransactionDto> EditTransactionAsync(TransactionDto c);
        Task DeleteTransactionAsync(TransactionDto c);

        Task<TransactionItemDto> AddTransactionItemAsync(TransactionItemDto ti);
        Task<TransactionItemDto> EditTransactionItemAsync(TransactionItemDto ti);
        Task DeleteTransactionItemAsync(TransactionItemDto ti);
    }
}