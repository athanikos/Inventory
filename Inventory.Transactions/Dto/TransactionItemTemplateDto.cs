namespace Inventory.Transactions.Dto
{
    public class TransactionItemTemplateDto
    {
        public required string Name { get; set; }

        public required DateTime Created {get; set; }
    }
}