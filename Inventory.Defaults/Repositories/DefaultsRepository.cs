using Inventory.Defaults.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Defaults.Repositories;

public class DefaultsRepository(DefaultsDbContext context) :IDefaultsRepository
{

    private const string CurrencyType = "CURRENCY";
    private const string StoreType = "STORE";

    public async Task<Default> GetDefaultCurrency()
    {
        return await context.Defaults.Where(i => i.Type == CurrencyType).SingleAsync();
    }

    public async Task<Default> GetDefaultStore()
    {
        return await context.Defaults.Where(i => i.Type == StoreType).SingleAsync();
    }

    public void InitializeDefaults()
    {
        // add store 
        
        
        
        //add curreency to UnitOfMeasurement  to get uuId 
        
        // then add it here 
        throw new NotImplementedException();
    }
}