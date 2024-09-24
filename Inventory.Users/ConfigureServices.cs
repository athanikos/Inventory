
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Inventory.Users
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, 
            IConfiguration configuration)
        {

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddDbContext<UsersDbContext>(options =>  
            options.UseNpgsql(configuration
            .GetValue<string>("Users")));

            services.AddIdentityCore<IdentityUser>().
            AddEntityFrameworkStores<UsersDbContext>().
            AddApiEndpoints();

            return services;
        }
    }
}
