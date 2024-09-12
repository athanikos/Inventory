namespace Inventory.Products.Contracts
{
    // todo becomes a class 
    public enum ModificationType
    {
        Buy = 0,
        Sell = 1,
        Let = 2
    }

    public static class ModificationTypeHelper
    {
        public static bool IsBuyOrSell(this ModificationType modificationType)
        {
            return modificationType == Contracts.ModificationType.Sell
                    || modificationType == Contracts.ModificationType.Let;
        }

        public static bool IsBuy(this ModificationType modificationType)
        {
            return modificationType == Contracts.ModificationType.Buy;
        }
    }
}