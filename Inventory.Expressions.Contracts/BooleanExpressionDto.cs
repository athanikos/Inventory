namespace Inventory.Expressions.Dto
{

    public record BooleanExpressionDto(Guid Id,
          string Expression,
          int RunEveryMinutes,
          Guid InventoryId);
}
