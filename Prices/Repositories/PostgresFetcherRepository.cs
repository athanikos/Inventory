namespace Inventory.Prices.Repositories
{
    public  class PostgresFetcherRepository: IFetcherRepository
    {
        private readonly PricesDbContext _context;

        public PostgresFetcherRepository(PricesDbContext context) 
        {
            _context = context;
        }  

        public   List<Entities.PricesParameter> GetParameters(  string _parameterType = "COINGECKO"
          )
        {
            if (string.IsNullOrEmpty(_parameterType))
                throw new ArgumentNullException(nameof(_parameterType));
            return [.. _context.Parameters.Where(p => p.ParameterType == _parameterType).ToList()];
        }

    }
}
