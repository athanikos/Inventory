using Inventory.Transactions.Dto;

namespace Inventory.Transactions.Repositories
{
    public interface ITransactionRepository
    {


        Task<TemplateDto> AddTemplateAsync(TemplateDto dto);
        Task<TemplateDto> EditTemplateAsync(TemplateDto dto);
        Task DeleteTemplateAsync(TemplateDto dto);
        Task<TemplateDto> GetTemplateAsync(Guid Id);

        Task<TransactionDto> AddTransactionAsync(TransactionDto dto);
        Task<TransactionDto> EditTransactionAsync(TransactionDto c);
        Task DeleteTransactionAsync(TransactionDto c);



        Task<TransactionDto> AddEntityAsync(EntityDto dto);
        Task<TransactionDto> EditEntityAsync(EntityDto c);
        Task DeleteEntityAsync(EntityDto c);

        //Task<FieldDto> AddFieldAsync(FieldDto dto);
        //Task<FieldDto> EditFieldAsync(FieldDto dto);
        //Task DeleteFieldAsync(FieldDto dto);
        //Task<FieldDto> GetFieldAsync(Guid Id);

        //Task<TransactionItemTemplateDto> AddTransactionItemTemplateAsync(TransactionItemTemplateDto dto);

        //Task<TransactionItemDto> AddTransactionItemAsync(TransactionItemDto ti);
        //Task<TransactionItemDto> EditTransactionItemAsync(TransactionItemDto ti);
        //Task DeleteTransactionItemAsync(TransactionItemDto ti);



    }
}