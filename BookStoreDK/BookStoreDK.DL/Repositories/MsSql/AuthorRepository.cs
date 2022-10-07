using System.Data.SqlClient;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookStoreDK.DL.Repositories.MsSql
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ILogger<AuthorRepository> _logger;
        private readonly IConfiguration _configuration;

        public AuthorRepository(ILogger<AuthorRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Author?> Add(Author model)
        {
            var query = @"INSERT INTO AUTHORS ([Name],Age,DateOfBirth,NickName)
                            VALUES (@Name,@Age,@DateOfBirth,@NickName)
                             SELECT CAST(SCOPE_IDENTITY() as int)";
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

            return null!;
        }

        public async Task<bool> AddMultipleAuthors(IEnumerable<Author> authors)
        {
            var query = @"INSERT INTO Authors VALUES (@Name, @Age,@DateOfBirth,@NickName)";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.ExecuteAsync(query, authors);

                    if (result == authors.Count())
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddMultipleAuthors)}:{e.Message}", e);
            }

            return false;
        }

        public async Task<Author?> Delete(int modelId)
        {
            var model = await GetById(modelId);

            var query = @"DELETE FROM AUTHORS
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

        public async Task<IEnumerable<Author>> GetAll()
        {
            var query = "prcGetAllAuthors";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QueryAsync<Author>(query);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAll)}:{e.Message}", e);
            }

            return Enumerable.Empty<Author>();
        }

        public async Task<Author?> GetAuthorByName(string name)
        {
            var query = @"SELECT * FROM Authors WITH (NOLOCK)
                          WHERE [Name] = @Name";

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QuerySingleOrDefaultAsync<Author>(query, new { Name = name });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAuthorByName)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Author?> GetById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<Author>("SELECT * FROM Authors WHERE Id = @Id", new { Id = id });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetById)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Author?> Update(Author model)
        {
            var query = @"UPDATE Authors
                        SET [NAME] = @Name, Age = @Age, DateOfBirth = @DateOfBirth, NickName = @NickName
                        WHERE Id = @Id
                        SELECT * FROM Authors
                        WHERE Id = @Id";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.QuerySingleAsync<Author>(query, model);
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
