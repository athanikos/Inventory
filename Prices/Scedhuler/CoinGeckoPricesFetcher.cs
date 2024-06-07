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
            


            internal virtual async Task FetchAsync()
            {




                var options = new RestClientOptions("https://api.coingecko.com/api/v3/coins/cardano");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("x-cg-demo-api-key", "CG-u6cMtcA6FGKaGxuJChCnCv5G\t");
                var response = await client.GetAsync(request);

                Console.WriteLine("{0}", response.Content);
            }
        }
        }
    }

