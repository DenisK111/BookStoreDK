using BookStoreDK.Models.Models;

namespace BookStoreDK.DL.Intefraces
{
    public interface IPersonRepository : IBaseRepository<Person, int>
    {
        Task<Person?> GetPersonByName(string name);
    }
}