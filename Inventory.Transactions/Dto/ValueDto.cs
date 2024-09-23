using Inventory.Transactions.Contracts;

namespace Inventory.Transactions.Dto
{

    public class ValueDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid FieldId { get; set; } = Guid.Empty;
        public string Text { get; set; } = string.Empty;
        public int GroupValue { get; set; }
        public Guid TransactionId { get; set; } = Guid.Empty;
        public SectionType TransactionSectionType { get; set; }

        //on new  auto gen by db based on group by ( sectionType,  groupvalue)     
        // on edit match by those Ids
       //    public Guid TransactionSectionId { get; set; } = Guid.Empty;
        public Guid TransactionSectionGroupId { get; set; } = Guid.Empty;
    
      
    }
}
