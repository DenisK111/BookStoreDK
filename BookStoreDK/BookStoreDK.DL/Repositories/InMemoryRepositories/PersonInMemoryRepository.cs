using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Repositories.InMemoryRepositories
{
    public class PersonInMemoryRepository : IPersonRepository
    {
        private static List<Person> _person = new List<Person>()
        {
            new Person()
            {
                Id = 1,
                Name="Pesho",
                Age = 20
            },
               new Person()
            {
                Id = 2,
                Name="Kerana",
                Age = 23
            },
                  new Person()
            {
                Id = 1,
                Name="Ivo",
                Age = 3
            }

        };


        public IEnumerable<Person> GetAll()
        {
            return _person;
        }

        public Person? GetById(int id)
        {
            return _person.FirstOrDefault(x => x.Id == id);
        }

        public Person? Add(Person user)
        {
            try
            {
                _person.Add(user);
            }
            catch (Exception a)
            {

                return null;
            }

            return user;

        }

        public Person? Update(Person user)
        {
            var existingPerson = _person.FirstOrDefault(x => x.Id == user.Id);

            if (existingPerson == null) return null;

            _person.Remove(existingPerson);
            _person.Add(user);

            return user;

        }

        public Person? Delete(int userId)
        {
            var person = _person.FirstOrDefault(user => user.Id == userId);
            _person.Remove(person);
            return person;
        }

    }
}
