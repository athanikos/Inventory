using Inventory.Defaults.Contracts;
using Inventory.Defaults.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Defaults.Repositories;

public class ConfigurationRepository(ConfigurationDbContext context) :IConfigurationRepository
{

  
    public async Task<Configuration> GetValue(ConfigurationType type )
    {
        return await context.Configurations.
            Where(i => i.Type == type).SingleAsync();
    }
    


}