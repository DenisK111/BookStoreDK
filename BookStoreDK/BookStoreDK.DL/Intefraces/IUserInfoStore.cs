using BookStoreDK.Models.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace BookStoreDK.DL.Intefraces
{
    public interface IUserInfoStore : IUserPasswordStore<UserInfo>
    {
        public Task<UserInfo?> GetUserInfoAsync(string email, string password);
    }
}
