namespace Inventory.Prices.Repositories
{
    public  interface IFetcherRepository
    {
        public List<Entities.PricesParameter> GetParameters(string _parameterType = "COINGECKO");
    }
}
