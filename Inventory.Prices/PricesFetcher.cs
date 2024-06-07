namespace Inventory.Prices
{
    public class PricesFetcher
    {
        private readonly  string  _targetUrl; 

        public PricesFetcher(string targetURL) 
        {
            _targetUrl = targetURL;    
        }
       



    }

    public class EbayPricesFetcher : PricesFetcher
    {
        public EbayPricesFetcher(string targetURL) : base(targetURL)
        {

        }
    }


    public class BrickLinkPricesFetcher : PricesFetcher
    {
        public BrickLinkPricesFetcher(string targetURL) : base(targetURL)
        {

        }
    }

    public class CoinGeckoPricesFetcher : PricesFetcher
    {
        public CoinGeckoPricesFetcher(string targetURL) : base(targetURL)
        {
        }
    }

}
