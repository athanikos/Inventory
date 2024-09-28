using Inventory.Defaults.Entities;

namespace Inventory.Defaults.Repositories;

public interface IDefaultsRepository
{
       Task<Default> GetDefaultCurrency();
       Task<Default> GetDefaultStore();  
       void InitializeDefaults();

}