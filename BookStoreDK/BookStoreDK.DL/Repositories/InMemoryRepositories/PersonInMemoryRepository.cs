using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Repositories.InMemoryRepositories
{
    public class PersonInMemoryRepository : IPersonRepository
    {
        private static List<Person> _users = new List<Person>()
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
            return _users;
        }

        public Person? GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public Person? Add(Person user)
        {
            try
            {
                _users.Add(user);
            }
            catch (Exception a)
            {

                return null;
            }

            return user;

        }

        public Person? Update(Person user)
        {
            var existingUser = _users.FirstOrDefault(x => x.Id == user.Id);

            if (existingUser == null) return null;

            _users.Remove(existingUser);
            _users.Add(user);

            return user;

        }

        public Person? Delete(int userId)
        {
            var user = _users.FirstOrDefault(user => user.Id == userId);
            _users.Remove(user);
            return user;
        }

    }
}
