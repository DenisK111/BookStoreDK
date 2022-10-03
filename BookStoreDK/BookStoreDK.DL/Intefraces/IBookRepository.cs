using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Intefraces
{
    public interface IBookRepository : IBaseRepository<Book,int>
    {
        public Book? GetBookByTitle(string title);
    }
}
