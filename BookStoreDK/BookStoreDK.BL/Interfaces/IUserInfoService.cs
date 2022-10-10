using BookStoreDK.Models.Models.Users;

namespace BookStoreDK.BL.Interfaces
{
    public interface IUserInfoService
    {
        public Task<UserInfo?> GetUserInfoAsync(string email, string password);
    }
}
