using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Repositories.InMemoryRepositories
{
    public class BookInMemoryRepository : IBookRepository
    {

        private static List<Book> _books = new()
        {
            new Book()
            {
                Id = 13,
                Title = "Patilansko Carstvo",
                AuthorId = 2
            }
            ,
             new Book()
            {
                Id = 1,
                Title = "Malechko",
                AuthorId = 1
            }
             ,
              new Book()
            {
                Id = 43,
                Title = "Bai Ganio",
                AuthorId = 3
            }
        };

        public IEnumerable<Book> GetAll()
        {
            return _books;
        }

        public Book? GetById(int id)
        {
            return _books.FirstOrDefault(x => x.Id == id);
        }

        public Book? Add(Book model)
        {
            try
            {
                _books.Add(model);
            }
            catch (Exception a)
            {

                return null;
            }

            return model;

        }

        public Book? Update(Book model)
        {
            var existingBook = _books.FirstOrDefault(x => x.Id == model.Id);

            if (existingBook == null) return null;

            _books.Remove(existingBook);
            _books.Add(model);

            return model;
        }

        public Book? Delete(int bookId)
        {
            var book = _books.FirstOrDefault(user => user.Id == bookId);
            _books.Remove(book!);
            return book;
        }
    }
}
