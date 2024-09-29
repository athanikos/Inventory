using Inventory.Defaults.Contracts;
using Inventory.Defaults.Entities;
using Inventory.Defaults.Repositories;

namespace Inventory.Defaults.Services;

public class ConfigurationService(IConfigurationRepository repo) : IConfigurationService
{
        public  async Task<Configuration> GetValue(ConfigurationType type )
        {
            return await repo.GetValue(type);
        }
}

public interface IConfigurationService
{

        Task<Configuration> GetValue(ConfigurationType type);

}