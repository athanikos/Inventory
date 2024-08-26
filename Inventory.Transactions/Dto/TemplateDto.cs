using Inventory.Transactions.Dto;

public record TemplateDto(   Guid Id, 
                             string Name ,
                             TemplateType Type,
                             DateTime Created, 
                             ICollection<FieldDto> fields
                             );

