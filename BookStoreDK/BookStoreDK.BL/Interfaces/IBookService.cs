using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Interfaces
{
    public interface IBookService
    {
        AddBookResponse? Add(AddBookRequest model);
        Book? Delete(int modelId);
        IEnumerable<Book> GetAll();
        Book? GetById(int id);
        UpdateBookResponse Update(UpdateBookRequest model);

        public Book? GetBookByName(string name);
    }
}
