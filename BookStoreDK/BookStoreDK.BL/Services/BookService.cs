using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;

namespace BookStoreDK.BL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo)
        {
            _repo = repo;
        }

        public Book? Add(Book model)
        {
            return _repo.Add(model);
        }

        public Book? Delete(int modelId)
        {
            return _repo.Delete(modelId);
        }

        public IEnumerable<Book> GetAll()
        {
            return _repo.GetAll();
        }

        public Book? GetById(int id)
        {
            return _repo.GetById(id);
        }

        public Book? Update(Book model)
        {
            return _repo.Update(model);
        }
    }
}
