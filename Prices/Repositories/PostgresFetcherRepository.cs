namespace Inventory.Prices.Repositories
{
    public class PostgresFetcherRepository(PricesDbContext context) : IFetcherRepository
    {
        public   List<Entities.PricesParameter> GetParameters(  string parameterType = "COINGECKO"
          )
        {
            if (string.IsNullOrEmpty(parameterType))
                throw new ArgumentNullException(nameof(parameterType));
            return [.. context.Parameters.Where(p => p.ParameterType == parameterType).ToList()];
        }

    }
}
