using Inventory.Transactions.Dto;

namespace Inventory.Transactions.Repositories
{
    public interface ITransactionRepository
    {
        Task EmptyDB();
        Task<TemplateDto> AddTemplateAsync(TemplateDto dto);
        Task<TemplateDto> EditTemplateAsync(TemplateDto dto);
        Task DeleteTemplateAsync(TemplateDto dto);
        Task<TemplateDto> GetTemplateAsync(Guid Id);

        Task<TransactionDto> AddTransactionAsync(TransactionDto dto);
        Task<TransactionDto> EditTransactionAsync(TransactionDto c);
        Task DeleteTransactionAsync(TransactionDto c);

    }
}