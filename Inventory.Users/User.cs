namespace Inventory.Users;
using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
