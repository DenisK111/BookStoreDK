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

        public Book? Add(Book user)
        {
            try
            {
                _books.Add(user);
            }
            catch (Exception a)
            {

                return null;
            }

            return user;

        }

        public Book? Update(Book user)
        {
            var existingBook = _books.FirstOrDefault(x => x.Id == user.Id);

            if (existingBook == null) return null;

            _books.Remove(existingBook);
            _books.Add(user);

            return user;

        }

        public Book? Delete(int userId)
        {
            var book = _books.FirstOrDefault(user => user.Id == userId);
            _books.Remove(book!);
            return book;
        }
    }
}
