using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Intefraces
{
    public interface IBookRepository : IBaseRepository<Book,int>
    {
        Task<Book?> GetBookByTitle(string title);
        Task<int> GetBooksCountByAuthorId(int AuthorId);
    }
}
