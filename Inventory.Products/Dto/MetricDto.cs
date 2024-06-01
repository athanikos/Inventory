namespace Inventory.Products.Dto;

public record MetricDto(Guid Id, 
                        string Description,
                        decimal Value,
                        DateTime  EffectiveDate,
                        string Code,
                        Guid SourceId  );

