using BookStoreDK.Models.Models;

namespace BookStoreDK.BL.Interfaces
{
    public interface IPersonService 
    {
        Person? Add(Person model);
        Person? Delete(int modelId);
        IEnumerable<Person> GetAll();
        Person? GetById(int id);
        Person? Update(Person model);
    }
}
