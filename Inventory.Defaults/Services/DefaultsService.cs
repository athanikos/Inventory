using Inventory.Defaults.Entities;

namespace Inventory.Defaults.Services;

public class DefaultsService : IDefaultsService
{

        public   Task<Default>  GetDefaultCurrency()
        {
                throw new NotImplementedException();
        }

        public     Task<Default>  GetDefaultStore()
        {
                throw new NotImplementedException();
        }
        
        public void InitializeDefaults()
        {
                 // call add unit of measurement add service  to add EURO as unit of measurementId 
                throw new NotImplementedException();
        }
}

public interface IDefaultsService
{

        Task<Default>   GetDefaultCurrency();
  
        Task<Default>  GetDefaultStore();

        void InitializeDefaults();

}