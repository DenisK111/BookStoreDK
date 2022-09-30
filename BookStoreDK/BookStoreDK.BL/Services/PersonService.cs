using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;

namespace BookStoreDK.BL.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repo;

        public PersonService(IPersonRepository repo)
        {
            _repo = repo;
        }

        public Person? Add(Person model)
        {
           return _repo.Add(model);
        }

        public Person? Delete(int modelId)
        {
            return _repo.Delete(modelId);
        }

        public IEnumerable<Person> GetAll()
        {
            return _repo.GetAll();
        }

        public Person? GetById(int id)
        {
            return _repo.GetById(id);
        }

        public Person? Update(Person model)
        {
            return _repo.Update(model);
        }
    }
}
