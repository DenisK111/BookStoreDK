using BookStoreDK.BL.Interfaces;
using BookStoreDK.Models.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace BookStoreDK.BL.Services
{
    public class IdentityService : IIDentityService
    {
        private readonly UserManager<UserInfo> _userManager;
        private readonly IPasswordHasher<UserInfo> _passwordHasher;

        public IdentityService(UserManager<UserInfo> userManager, IPasswordHasher<UserInfo> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
           
        }

        public async Task<UserInfo?> CheckUserAndPassword(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return null;
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            return result == PasswordVerificationResult.Success ? user : null;


        }

        public async Task<IdentityResult> CreateAsync(UserInfo user)
        {
            var exUser = await _userManager.GetUserIdAsync(user);

            if (exUser == null)
            {
                return await _userManager.CreateAsync(user);
            }

            return IdentityResult.Failed();
        }

        public async Task<IEnumerable<string>> GetRoles(UserInfo user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles;
        }
    }
}
