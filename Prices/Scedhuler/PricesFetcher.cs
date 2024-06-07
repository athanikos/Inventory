using Inventory.Prices;
using Inventory.Prices.Entities;

namespace Prices
{
    namespace Inventory.Prices
    {
        internal abstract class PricesFetcher
        {
            protected string _parameterType = string.Empty;
            protected readonly PricesDbContext _context;
           
            internal  PricesFetcher(PricesDbContext context)
            {
                _context = context; 
            }

            protected List<Parameters> FetchParameters()
            {
                    throw new NotImplementedException();
            }
           

        }

    }
}
