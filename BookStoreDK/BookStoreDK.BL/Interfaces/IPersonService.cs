using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Interfaces
{
    public interface IPersonService 
    {
        AddPersonResponse Add(AddPersonRequest model);
        Person? Delete(int modelId);
        IEnumerable<Person> GetAll();
        Person? GetById(int id);
        UpdatePersonResponse Update(UpdatePersonRequest model);
    }
}
