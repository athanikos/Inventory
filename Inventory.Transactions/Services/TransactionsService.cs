namespace Inventory.Transactions.Services
{
    using Inventory.Products.Contracts;
    using Dto;
    using Repositories;
    using MediatR;
    public class TransactionsService(
        ITransactionRepository repository,
        IMediator mediator)
        : ITransactionService
    {
        public async Task DeleteTransactionAsync(TransactionDto c)
        {
            throw new NotImplementedException();
            await Task.CompletedTask;
        }

        public async Task<TemplateDto> EditTemplateAsync(TemplateDto inboundTemplate)
        {
             return await   repository.EditTemplateAsync(inboundTemplate);
        }

        public async Task EmptyDb()
        {
           await  repository.EmptyDb();
        }

        public async Task<Guid> RoomsPrepareAsync()
        {
            return await  repository. RoomsPrepareAsync();
        }

        
        
        public async Task<TransactionDto> GetValuesForNewTransaction(Guid templateId)
        {
            var template = await repository.GetTemplateAsync(templateId);
            TransactionDto trans = new TransactionDto(Guid.NewGuid());

            var transactionSections = template.
                        Sections.
                        Select(
                                 s => new TransactionSectionDto()
                                 {
                                     Id = Guid.NewGuid(),
                                     Name = s.Name,
                                     TransactionId = Guid.NewGuid(),
                                     TransactionSectionType = s.SectionType,
                                     SectionGroups =
                                     [
                                                       new TransactionSectionGroupDto()
                                                       {
                                                         Id = Guid.NewGuid(),
                                                         GroupValue =0,
                                                         Values =
                                                         [ ..
                                                                    s.Fields.Select
                                                                    (
                                                                        u=> new ValueDto()
                                                                        {
                                                                            Id = u.Id,
                                                                            Text = string.Empty,
                                                                            TransactionId = trans.Id,
                                                                            TransactionSectionGroupId =
                                                                            Guid.NewGuid()
                                                                        }
                                                                    ),
                                                         ]
                                                       }
                                     ],
                                 }
                        ).ToList();

            foreach (var item in transactionSections)
                trans.Sections.Add(item);

            return trans;
        }
        
        public async Task<TransactionDto> UpdateOrInsertTransaction(TransactionDto transaction)
        {
            if (transaction.Id == Guid.Empty)
                return await repository.AddTransactionAsync(transaction);
            return await repository.EditTransactionAsync(transaction);
        }
        
        public async Task<TransactionDto> CancelTransaction(TransactionDto transaction)
        {
            var t = await repository.GetTransactionAsync(transaction.Id);
            t.StatusId = Contracts.TransactionStatus.Cancelled;
            await repository.EditTransactionAsync(t);
            var command = new CancelQuantityMetricCommand(transaction.Id);
            await mediator.Send(command);
            return await repository.GetTransactionAsync(t.Id);

        }

        public async  Task<TemplateDto> AddTemplateAsync(TemplateDto templateDto)
        {
            return await repository.AddTemplateAsync(templateDto);
        }

        public async Task DeleteTemplateAsync(TemplateDto c)
        {
            await repository.DeleteTemplateAsync(c);
        }
    }
}
