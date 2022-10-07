using Microsoft.AspNetCore.Identity;

namespace BookStoreDK.Models.Models.Users
{
    public class UserRole : IdentityRole
    {
        public int UserId { get; set; }
    }
}
