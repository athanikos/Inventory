using Inventory.Defaults.Contracts;
using Inventory.Defaults.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Defaults.Repositories;

public class ConfigurationRepository(ConfigurationDbContext context) :IConfigurationRepository
{

    public async Task SaveAsync (List<Configuration> items)
    {
        foreach (var i in items)
            context.Add(i);
        await context.SaveChangesAsync();
    }

    public void  EmptyDb()
    {
        context.Configurations.RemoveRange(context.Configurations);
        context.SaveChanges();
    }
    
    public async Task<Configuration> GetValue(ConfigurationType type )
    {
        return await context.Configurations.
            Where(i => i.Type == type).SingleAsync();
    }
    


}