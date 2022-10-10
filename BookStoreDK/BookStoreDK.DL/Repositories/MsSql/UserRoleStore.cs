using System.Data.SqlClient;
using BookStoreDK.Models.Models.Users;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookStoreDK.DL.Repositories.MsSql
{
    public class UserRoleStore : IRoleStore<UserRole>
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<UserRoleStore> _logger;

        public UserRoleStore(IConfiguration configuration, ILogger<UserRoleStore> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IdentityResult> CreateAsync(UserRole role, CancellationToken cancellationToken)
        {
            var query = @"INSERT INTO UserRoles
                                   ([Id],[RoleId],UserId])
                                    Values
                                    (@Id,
                                      @RoleId,
                                      @UserId)";
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    await conn.OpenAsync(cancellationToken);

                    var result = await conn.ExecuteAsync(query, role);

                    return IdentityResult.Success;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error in {nameof(UserRoleStore.CreateAsync)}: {e.Message}");
                    return IdentityResult.Failed();
                }

            }
        }

        public Task<IdentityResult> DeleteAsync(UserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<UserRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var query = @"SELECT ur.*, r.RoleName as Name FROM UserRoles ur WITH (NOLOCK)
                        JOIN Roles as r WITH (NOLOCK) on ur.RoleId = r.Id
                        WHERE r.RoleName = @Name";
            await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    await conn.OpenAsync(cancellationToken);

                    var result = await conn.QueryFirstOrDefault(query, new {Name = normalizedRoleName});

                    return result;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error in {nameof(UserRoleStore.CreateAsync)}: {e.Message}");
                    return null;
                }

            }
        }

        public Task<string> GetNormalizedRoleNameAsync(UserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(UserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(UserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(UserRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(UserRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(UserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
