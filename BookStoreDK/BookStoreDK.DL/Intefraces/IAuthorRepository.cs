using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Intefraces
{
    public interface IAuthorRepository : IBaseRepository<Author, int>
    {
        Task<Author?> GetAuthorByName(string name);

        Task<bool> AddMultipleAuthors(IEnumerable<Author> authors);
    }
}
