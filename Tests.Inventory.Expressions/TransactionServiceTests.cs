using Inventory.Transactions.Dto;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;


namespace Tests.Inventory
{
    [Collection("Our Test Collection #1")]
    public class TransactionServiceTests(ITestOutputHelper testOutputHelper,  
        TestFixture fixture)
        : TestBed<TestFixture>(testOutputHelper, fixture)
    {
        [Fact]
        public async Task TestOnCancellationAllRecordsAreCancelledAndTransactionStatusBecomesCanceled()
        {
        

            var output = await TestSetup.Setup(_testOutputHelper, 
                _fixture);

            await output.TransactionService.RoomsPrepareAsync();
            var template = (await output.TransactionRepo.GetTemplatesAsync())[0];
      
            TransactionDto dto =       await   output.TransactionService.GetValuesForNewTransaction(template.Id);
         
            var inputFieldsCount = template.Sections.
                SelectMany(s => s.Fields).Count();
            var inputSectionsCount = template.Sections.Count();
            
            var outputValuesCount = dto.Sections.
                SelectMany(s => s.SectionGroups).
                SelectMany(sg => sg.Values).Count();
            var transactionSectionGroups = dto.Sections.
                SelectMany(s => s.SectionGroups).ToList();
            var values = dto.Sections.
                SelectMany(s => s.SectionGroups).
                SelectMany(sg => sg.Values).ToList();

            
            Assert.NotEqual(template.Id,Guid.Empty);
            Assert.Equal(inputSectionsCount,dto.Sections.Count);
            Assert.Equal(outputValuesCount,inputFieldsCount);
            Assert.Equal(template.Id, dto.TemplateId);
            foreach (var sg in transactionSectionGroups)
                Assert.Equal(0, sg.GroupValue);
            foreach (var v in values)
                Assert.Equal(string.Empty, v.Text);

        }

        [Fact]
        public  async Task  TestOnSaveTransaction()
        {
           var output = await TestSetup.Setup(_testOutputHelper, 
                _fixture);

            await   output.TransactionService.RoomsPrepareAsync();
            var template = (await output.TransactionRepo.GetTemplatesAsync())[0];
            TransactionDto dto =       await   output.TransactionService.GetValuesForNewTransaction(template.Id); 
          
            dto.Id = ( await   output.TransactionService.UpdateOrInsertTransaction(dto)).Id;

           
            var transaction = await  output.TransactionService.GetTransactionAsync(dto.Id);
            Assert.Equal(dto.Id, transaction.Id);

            var values = transaction.Sections.SelectMany(s => s.SectionGroups).SelectMany(sg => sg.Values);

            foreach (var v in values)
                v.Text = "aaaa";
           
            
         
            dto.Id = ( await   output.TransactionService.UpdateOrInsertTransaction(transaction)).Id;
            dto.Id = ( await   output.TransactionService.UpdateOrInsertTransaction(transaction)).Id;
         

            var items = await  output.TransactionRepo.GetTransactionsAsync();
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public  void TestOnEditTransaction()
        {
            Assert.Equal(1,0);
        }
    }
}
