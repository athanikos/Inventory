
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
            var connectionString = configuration.GetConnectionString("Users");


         

            services.AddDbContext<UsersDbContext>(options =>
                                                   options.UseSqlServer(connectionString)
                
                );

            services.AddIdentityCore<IdentityUser>().
             AddEntityFrameworkStores<UsersDbContext>().
             AddApiEndpoints();


            //services.AddScoped<UsersDbContext>(provider
            //    => provider.GetRequiredService<UsersDbContext>());


            return services;
        }
    }
}
