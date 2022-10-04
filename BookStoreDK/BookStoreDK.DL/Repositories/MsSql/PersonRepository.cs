using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Repositories.MsSql
{
    public class PersonRepository : IPersonRepository
    {
        public Task<Person?> Add(Person model)
        {
            throw new NotImplementedException();
        }

        public Task<Person?> Delete(int modelId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Person>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Person?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Person?> GetPersonByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Person?> Update(Person model)
        {
            throw new NotImplementedException();
        }
    }
}
