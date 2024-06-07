using Inventory.Prices;

namespace Prices
{
    namespace Inventory.Prices
    {
        internal class BrickLinkPricesFetcher : PricesFetcher
        {
            internal BrickLinkPricesFetcher(PricesDbContext context) : base(context)
            {
                _parameterType = "COINGECKO";
            }
        }

    }
}
