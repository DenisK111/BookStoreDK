using BookStoreDK.Models.Models;

namespace BookStoreDK.BL.Interfaces
{
    public interface IBookService
    {
        Book? Add(Book model);
        Book? Delete(int modelId);
        IEnumerable<Book> GetAll();
        Book? GetById(int id);
        Book? Update(Book model);
    }
}
