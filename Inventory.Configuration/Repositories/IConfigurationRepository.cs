using Inventory.Defaults.Contracts;
using Inventory.Defaults.Entities;

namespace Inventory.Defaults.Repositories;

public interface IConfigurationRepository
{
         Task<Configuration> GetValue(ConfigurationType type);

}