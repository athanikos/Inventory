namespace Inventory.Expressions.Dto;

public record ProductExpressionDto(Guid Id,
          string Expression,
          int RunEveryMinutes,
          Guid TargetProductId,
          Guid TargetMetricId,
          Guid InventoryId);