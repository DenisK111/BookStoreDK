using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models;

namespace BookStoreDK.BL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repo;

        public AuthorService(IAuthorRepository repo)
        {
            _repo = repo;
        }

        public Author? Add(Author model)
        {
            return _repo.Add(model);
        }

        public Author? Delete(int modelId)
        {
            return _repo.Delete(modelId);
        }

        public IEnumerable<Author> GetAll()
        {
            return _repo.GetAll();
        }

        public Author? GetById(int id)
        {
            return _repo.GetById(id);
        }

        public Author? Update(Author model)
        {
            return _repo.Update(model);
        }
    }
}
