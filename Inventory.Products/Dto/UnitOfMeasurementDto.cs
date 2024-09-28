using Inventory.Products.Contracts;

namespace Inventory.Products.Dto;

public record UnitOfMeasurementDto(Guid Id, string Text, UnitOfMeasurementType Type);
