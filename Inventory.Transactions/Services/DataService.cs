using Inventory.Transactions.Dto;
using Inventory.Transactions.Repositories;

namespace Inventory.Transactions.Services
{
    public  class DataService
    {
        private ITransactionRepository _transactionRepository;

        private DataService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

       

        public async Task  Initialize()
        {
            // add template 

            List<SectionDto> sections = new List<SectionDto>();

            sections.Add(new SectionDto()
            {
                Id = Guid.NewGuid(),
                Name = "Rooms Section",
                SectionType = Contracts.SectionType.ProductLet,

                Fields = new List<FieldDto> 
                { 
                    new FieldDto()
                    { Expression = string.Empty,
                      Name="DateFrom",
                      Type= FieldType.Date
                    },
                     new FieldDto()
                    { Expression = string.Empty,
                      Name="DateTo",
                      Type= FieldType.Date
                    },
                      new FieldDto()
                    { Expression = "DateTo - DateFrom",
                      Name="Days",
                      Type= FieldType.Date
                    },
                }
            });

            sections.Add(new SectionDto()
            {
                Id = Guid.NewGuid(),
                Name = "Food & Beverages Section",
                SectionType = Contracts.SectionType.ProductSell,

    
                
            });


            var t = await _transactionRepository.AddTemplateAsync(new TemplateDto(Guid.NewGuid(),
                                                         "Rooms Transactions Template", TemplateType.Transaction,
                                                         DateTime.UtcNow, sections
            ));

        }


    }
}
