using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inventory.Users
{
    public class UsersDbContext : IdentityDbContext<IdentityUser>
    {
        public UsersDbContext (DbContextOptions<UsersDbContext> options) 
            : base(options) {
        
        
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Users");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
