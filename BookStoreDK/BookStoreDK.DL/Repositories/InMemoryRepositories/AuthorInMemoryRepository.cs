using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Repositories.InMemoryRepositories
{
    public class AuthorInMemoryRepository : IAuthorRepository


    {

        private static List<Author> _authors = new List<Author>()
        {
            new Author()
            {
                Id = 1,
                Name="Ivo",
                Age = 20,
                DateOfBirth= new DateTime(1990,01,15),
                NickName = "Ivcho",
            },
               new Author()
            {
                Id = 2,
                Name="Ivan",
                Age = 23,
                DateOfBirth= new DateTime(1994,02,11),
                NickName = "Ivanko",
            },
                  new Author()
            {
                Id = 1,
                Name="Misho",
                Age = 3,
               DateOfBirth= new DateTime(1991,03,16),
                NickName = "Micho",
            }

        };

        public IEnumerable<Author> GetAll()
        {
            return _authors;
        }

        public Author? GetById(int id)
        {
            return _authors.FirstOrDefault(x => x.Id == id);
        }

        public Author? Add(Author user)
        {
            try
            {
                _authors.Add(user);
            }
            catch (Exception a)
            {

                return null;
            }

            return user;

        }

        public Author? Update(Author user)
        {
            var existingAuthor = _authors.FirstOrDefault(x => x.Id == user.Id);

            if (existingAuthor == null) return null;

            _authors.Remove(existingAuthor);
            _authors.Add(user);

            return user;

        }

        public Author? Delete(int userId)
        {
            var author = _authors.FirstOrDefault(user => user.Id == userId);
            _authors.Remove(author!);
            return author;
        }

        public Author? GetAuthorByName(string name)
        {
            return _authors.FirstOrDefault(x => x.Name == name);
        }
    }
}
