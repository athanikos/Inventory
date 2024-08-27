namespace Inventory.Transactions.Dto
{
    public class ValueDto
    {
        public Guid Id { get; set; }

        public required FieldDto Field { get; set; }

        public string Text { get; set; } = string.Empty;
    }
}
