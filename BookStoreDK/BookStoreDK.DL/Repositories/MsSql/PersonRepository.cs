using System.Data.SqlClient;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookStoreDK.DL.Repositories.MsSql
{
    public class PersonRepository : IPersonRepository
    {

        private readonly ILogger<PersonRepository> _logger;
        private readonly IConfiguration _configuration;

        public PersonRepository(ILogger<PersonRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Person?> Add(Person model)
        {
            var query = @"INSERT INTO Person ([Name],Age,DateOfBirth)
                            VALUES (@Name,@Age,@DateOfBirth)
                             SELECT CAST(SCOPE_IDENTITY() as int) WITH(NOLOCK)";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.QuerySingleAsync<int>(query, model);
                    model.Id = result;
                    return model;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Add)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Person?> Delete(int modelId)
        {
            var model = await GetById(modelId);

            var query = @"DELETE FROM PERSON
                          WHERE Id = @Id";

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    await conn.ExecuteAsync(query, new { Id = modelId });
                    return model;
                }
            }

            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Delete)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            var query = "SELECT * FROM PERSON WITH (NOLOCK)";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QueryAsync<Person>(query);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAll)}:{e.Message}", e);
            }

            return Enumerable.Empty<Person>();
        }

        public async Task<Person?> GetById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<Person>("SELECT * FROM Person WHERE Id = @Id", new { Id = id });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetById)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Person?> GetPersonByName(string name)
        {
            var query = @"SELECT * FROM Person WITH(NOLOCK)
                          WHERE [Name] = @Name";

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QuerySingleOrDefaultAsync<Person>(query, new { Name = name });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetPersonByName)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Person?> Update(Person model)
        {
            var query = @"UPDATE Person
                        SET [NAME] = @Name, Age = @Age, DateOfBirth = @DateOfBirth
                        WHERE Id = @Id
                        SELECT * FROM Person
                        WHERE Id = @Id";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.QuerySingleAsync<Person>(query, model);
                    return model;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Update)}:{e.Message}", e);
            }

            return null;
        }
    }
}
