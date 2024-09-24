using Inventory.Transactions.Dto;

namespace Inventory.Transactions.Services
{
    public interface ITransactionService
    {
        Task<TransactionDto> GetValuesForNewTransaction(Guid templateId);
        Task<TransactionDto> UpdateOrInsertTransaction(TransactionDto transaction);
        Task<TransactionDto> CancelTransaction(TransactionDto transaction);



    }
}
