using BookStoreDK.Models.Models;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.DL.Intefraces
{
    public interface IBookRepository : IBaseRepository<Book,int>
    {
        Task<Book?> GetBookByTitle(string title);
        Task<int> GetBooksCountByAuthorId(int AuthorId);
    }
}
