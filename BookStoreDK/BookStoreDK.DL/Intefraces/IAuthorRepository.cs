using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Intefraces
{
    public interface IAuthorRepository : IBaseRepository<Author,int>
    {
        public Author? GetAuthorByName(string name);

        bool AddMultipleAuthors(IEnumerable<Author> authors);
    }
}
