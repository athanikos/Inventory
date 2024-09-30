using Inventory.Defaults.Contracts;
using Inventory.Defaults.Entities;
using Inventory.Defaults.Repositories;
using Inventory.Products.Contracts;

namespace Inventory.Defaults.Services;

public class ConfigurationService(IConfigurationRepository repo) : IConfigurationService
{
        public  async Task<Configuration> GetValueAsync(ConfigurationType type )
        {
            return await repo.GetValue(type);
        }

        public void EmptyDb()
        {
               repo.EmptyDb(); 
        }
        
        public async Task SaveAsync(List<InitializeConfigurationResponse> items)
        {
                var configs =
                        items.Select(o =>
                                new Configuration()
                                {
                                        Type = o.TypeName,
                                        Value = o.Value,
                                        EntityId = o.Id
                                }).ToList();

                await repo.SaveAsync(configs);
        }
        
}

public interface IConfigurationService
{
         void EmptyDb();
        

        Task<Configuration> GetValueAsync(ConfigurationType type);

        Task SaveAsync(List<InitializeConfigurationResponse> items);

}