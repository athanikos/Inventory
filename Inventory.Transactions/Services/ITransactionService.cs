using Inventory.Transactions.Dto;

namespace Inventory.Transactions.Services
{
    public interface ITransactionService
    {
        Task<TransactionDto> GetValuesForNewTransaction(Guid templateId);
        Task<TransactionDto> UpdateOrInsertTransaction(TransactionDto transaction);
        Task<TransactionDto> CancelTransaction(TransactionDto transaction);

        Task DeleteTransactionAsync(TransactionDto c);
        
        Task DeleteTemplateAsync(TemplateDto c);
        Task<TemplateDto> AddTemplateAsync(TemplateDto templateDto);

         Task<TemplateDto> EditTemplateAsync(TemplateDto inboundTemplate);

         Task EmptyDb();
         
         Task<Guid> RoomsPrepareAsync();

    }
}
