using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookStoreDK.DL.Repositories.MsSql
{
    public class BookRepository : IBookRepository
    {

        private readonly ILogger<BookRepository> _logger;
        private readonly IConfiguration _configuration;

        public BookRepository(ILogger<BookRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Book?> Add(Book model)
        {
            var query = @"INSERT INTO BOOKS (AuthorId,Title,LastUpdated,Quantity,Price)
                          VALUES (@AuthorId,@Title, GetDate() ,@Quantity,@Price)
                          SELECT * FROM BOOKS WHERE Id = (SELECT CAST(SCOPE_IDENTITY() as int))";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.QuerySingleAsync<Book>(query, model);
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Add)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Book?> Delete(int modelId)
        {
            var model = await GetById(modelId);

            var query = @"DELETE FROM Books
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

        public async Task<IEnumerable<Book>> GetAll()
        {
            var query = "SELECT * FROM BOOKS WITH (NOLOCK)";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QueryAsync<Book>(query);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAll)}:{e.Message}", e);
            }

            return Enumerable.Empty<Book>();
        }

        public async Task<Book?> GetBookByTitle(string title)
        {
            var query = @"SELECT * FROM BOOKS WITH(NOLOCK)
                          WHERE [Title] = @Title";

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QuerySingleOrDefaultAsync<Book>(query, new { Title = title });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetBookByTitle)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<int> GetBooksCountByAuthorId(int authorId)
        {
            var query = @"SELECT Count(*) FROM Books
                        WHERE AuthorId = @AuthorId";

            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.ExecuteScalarAsync<int>(query, new { AuthorId = authorId });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetById)}:{e.Message}", e);
            }

            return default;


        }

        public async Task<Book?> GetById(int id)
        {


            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    return await conn.QueryFirstOrDefaultAsync<Book>("SELECT * FROM Books WHERE Id = @Id", new { Id = id });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetById)}:{e.Message}", e);
            }

            return null;
        }

        public async Task<Book?> Update(Book model)
        {


            var query = @"UPDATE Books
                        SET [AuthorId] = @AuthorId, Title = @Title, LastUpdated = GetDate(), Quantity = @Quantity, Price = @Price
                        WHERE Id = @Id
                        SELECT * FROM Books
                        WHERE Id = @Id";
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var result = await conn.QuerySingleAsync<Book>(query, model);
                    return result;
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
