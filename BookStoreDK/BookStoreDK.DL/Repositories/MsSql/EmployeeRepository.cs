using System.Data.SqlClient;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models.Users;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookStoreDK.DL.Repositories.MsSql
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly IConfiguration _configuration;

        public EmployeeRepository(ILogger<EmployeeRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task AddEmployee(Employee employee)
        {
            var query = @"INSERT INTO Employee 
                        ([EmployeeID]
                       ,[NationalIDNumber]
                       ,[EmployeeName]
                       ,[LoginID]
                       ,[JobTitle]
                       ,[BirthDate]
                       ,[MaritalStatus]
                       ,[Gender]
                       ,[HireDate]
                       ,[VacationHours]
                       ,[SickLeaveHours]
                       ,[rowguid]
                       ,[ModifiedDate])
                        VALUES 
                        (@EmployeeID
                       ,@NationalIDNumber
                       ,@EmployeeName
                       ,@LoginID
                       ,@JobTitle
                       ,@BirthDate
                       ,@MaritalStatus
                       ,@Gender
                       ,@HireDate
                       ,@VacationHours
                       ,@SickLeaveHours
                       ,@rowguid
                       ,@ModifiedDate)";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.ExecuteAsync(query, employee);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddEmployee)}:{e.Message}", e);
            }

        }

        public async Task<bool> CheckEmployee(int id)
        {
            var query = @"SELECT COUNT(1)
                            FROM Employee
                            WHERE EmployeeId = @EmployeeId";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.ExecuteScalarAsync<int>(query, new { EmployeeId = id });
                    return result > 0;
                }
            }

            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(CheckEmployee)}:{e.Message}", e);
            }

            return false;

        }

        public async Task DeleteEmployee(int id)
        {

            var query = @"DELETE FROM Employee
                          WHERE EmployeeId = @EmployeeId";

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    await conn.ExecuteAsync(query, new { EmployeeId = id });

                }
            }

            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteEmployee)}:{e.Message}", e);
            }

        }

        public async Task<IEnumerable<Employee>> GetEmployeeDetails()
        {
            var query = "SELECT * FROM EMPLOYEE WITH (NOLOCK)";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QueryAsync<Employee>(query);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetEmployeeDetails)}:{e.Message}", e);
            }

            return Enumerable.Empty<Employee>();
        }

        public async Task<Employee?> GetEmployeeDetails(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<Employee>("SELECT * FROM Employee WHERE EmployeeId = @EmployeeId", new { EmployeeId = id });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetEmployeeDetails)}:{e.Message}", e);
            }

            return null;
        }


        public async Task UpdateEmployee(Employee employee)
        {
            string query =
                       @"UPDATE [dbo].[Employee]
                       SET [EmployeeID] = @EmployeeID
                          ,[NationalIDNumber] = @NationalIDNumber
                          ,[EmployeeName] = @EmployeeName
                          ,[LoginID] = @LoginID
                          ,[JobTitle] = @JobTitle
                          ,[BirthDate] = @BirthDate
                          ,[MaritalStatus] = @MaritalStatus
                          ,[Gender] = @Gender
                          ,[HireDate] = @HireDate
                          ,[VacationHours] = @VacationHours
                          ,[SickLeaveHours] = @SickLeaveHours
                          ,[rowguid] = @rowguid
                          ,[ModifiedDate] = @ModifiedDate
                     WHERE EmployeeID = @EmployeeID";

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.ExecuteAsync(query, employee);
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateEmployee)}:{e.Message}", e);
            }

            return;
        }
    }
}
