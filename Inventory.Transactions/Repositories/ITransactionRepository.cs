using Inventory.Transactions.Dto;

namespace Inventory.Transactions.Repositories
{
    public interface ITransactionRepository
    {
        TransactionsDbContext Context();

        Task EmptyDb();
        Task<TemplateDto> AddTemplateAsync(TemplateDto dto);
        Task<TemplateDto> EditTemplateAsync(TemplateDto dto);
        Task DeleteTemplateAsync(TemplateDto dto);
        Task<TemplateDto> GetTemplateAsync(Guid id);

        Task<TransactionDto> AddTransactionAsync(TransactionDto dto);
        Task<TransactionDto> EditTransactionAsync(TransactionDto dto);
        Task DeleteTransactionAsync(TransactionDto dto);
        Task<TransactionDto> GetTransactionAsync(Guid id);

        Task<List<TransactionDto>> GetTransactionsAsync();

        Task<Guid> RoomsPrepareAsync();

        Task<List<TemplateDto>> GetTemplatesAsync();
    }
}