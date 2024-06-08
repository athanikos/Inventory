using Inventory.Prices;
using RestSharp;

namespace Prices
{
    namespace Inventory.Prices
    {
        internal class CoinGeckoPricesFetcher : PricesFetcher
        {
            internal CoinGeckoPricesFetcher(PricesDbContext context) :base (context) 
            {
                _parameterType = "COINGECKO";
            }
        }
    }
    }

