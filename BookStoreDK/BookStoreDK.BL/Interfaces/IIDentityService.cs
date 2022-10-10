using BookStoreDK.Models.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace BookStoreDK.BL.Interfaces
{
    public interface IIDentityService
    {
        Task<IdentityResult> CreateAsync(UserInfo user);

        Task<UserInfo?> CheckUserAndPassword(string userName, string password);

        Task<IEnumerable<string>> GetRoles(UserInfo user);
    }
}
