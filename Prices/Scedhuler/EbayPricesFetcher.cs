using Inventory.Prices;

namespace Prices
{
    namespace Inventory.Prices
    {
        internal class EbayPricesFetcher : PricesFetcher
        {
            internal EbayPricesFetcher(PricesDbContext context) : base(context)
            {
                _parameterType = "COINGECKO";
            }

        }
    }
}
