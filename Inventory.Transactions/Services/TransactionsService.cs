namespace Inventory.Transactions.Services
{
    using Inventory.Products.Contracts;
    using Inventory.Transactions.Dto;
    using Inventory.Transactions.Repositories;
    using MediatR;


    public class TransactionsService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private IMediator _mediator;


        public TransactionsService(ITransactionRepository repository,
                                   IMediator mediator)
        {
            _transactionRepository = repository;
            _mediator = mediator;   
        }

        public async Task<TransactionDto> GetValuesForNewTransaction(Guid templateId)
        {
            var template = await _transactionRepository.GetTemplateAsync(templateId);
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
                return await _transactionRepository.AddTransactionAsync(transaction);
            else
                return await _transactionRepository.EditTransactionAsync(transaction);
        }



        public async Task<TransactionDto> CancellTransaction(TransactionDto transaction)
        {
            var t = await _transactionRepository.GetTransactionAsync(transaction.Id);
            t.StatusId = Contracts.TransactionStatus.Cancelled;
            await _transactionRepository.EditTransactionAsync(t);
            var command = new CancellQuantityMetricCommand(transaction.Id);
            await _mediator.Send(command);
            return await _transactionRepository.GetTransactionAsync(t.Id);

        }
    }
}
