

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Users
{
    public class UsersDbContext : IdentityDbContext<User>
    {
   
        public UsersDbContext (DbContextOptions<UsersDbContext> options) 
            : base(options) {
        
        
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Property(u => u.Initials).HasMaxLength(5);

            builder.HasDefaultSchema("Users");
        }

    }
}
