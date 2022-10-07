using System.Data.SqlClient;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models.Users;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookStoreDK.DL.Repositories.MsSql
{
    public class UserInfoStore : IUserInfoStore
    {
        private readonly ILogger<UserInfoStore> _logger;
        private readonly IConfiguration _configuration;

        public UserInfoStore(ILogger<UserInfoStore> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CreateAsync(UserInfo user, CancellationToken cancellationToken)
        {
            var query = @"INSERT INTO UserInfo 
                        ([UserId]
                       ,[UserName]
                       ,[Email]
                       ,[Password]
                       ,[CreatedDate]
                       VALUES 
                        (@UserId
                       ,@UserName
                       ,@Email
                       ,@Password
                       ,@CreatedDate)";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.ExecuteAsync(query, user);
                    return IdentityResult.Success;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(CreateAsync)}:{e.Message}", e);
            }

            return IdentityResult.Failed(new IdentityError()
            {
                Description = $"Error in{nameof(CreateAsync)}"
            });
        }

        public Task<IdentityResult> DeleteAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<UserInfo> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInfo> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var query = @"SELECT * FROM UserInfo WITH (NOLOCK)
                          WHERE UserName = @UserName";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.QueryFirstOrDefaultAsync<UserInfo>(query, normalizedUserName);
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(FindByNameAsync)}:{e.Message}", e);
            }

            return null!;
        }

        public Task<string> GetNormalizedUserNameAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetPasswordHashAsync(UserInfo user, CancellationToken cancellationToken)
        {
            return user.Password;
        }

        public Task<string> GetUserIdAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInfo?> GetUserInfoAsync(string email, string password)
        {
            var query = @"SELECT * FROM UserInfo WITH (NOLOCK)
                           WHERE Email = @Email AND Password = @Password";

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<UserInfo>(query, new { Email = email, Password = password });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetUserInfoAsync)}:{e.Message}", e);
            }

            return null;
        }

        public Task<string> GetUserNameAsync(UserInfo user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(UserInfo user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetPasswordHashAsync(UserInfo user, string passwordHash, CancellationToken cancellationToken)
        {
            var query = @"UPDATE UserInfo
                          SET Password = @Password
                          WHERE UserId = @UserId";

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                   await conn.ExecuteAsync(query, new { Password = passwordHash, UserId = user.UserId });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetUserInfoAsync)}:{e.Message}", e);
            }

           
        }

        public Task SetUserNameAsync(UserInfo user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(UserInfo user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
