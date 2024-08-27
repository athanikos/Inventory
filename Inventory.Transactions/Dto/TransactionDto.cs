namespace Inventory.Transactions.Dto;

public record TransactionDto(Guid Id, string Description, DateTime Created, ICollection<ValueDto> Values);



