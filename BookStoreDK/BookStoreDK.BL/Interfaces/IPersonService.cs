using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Interfaces
{
    public interface IPersonService
    {
        Task<AddPersonResponse> Add(AddPersonRequest model);
        Task<Person?> Delete(int modelId);
        Task<IEnumerable<Person>> GetAll();
        Task<Person?> GetById(int id);
        Task<UpdatePersonResponse> Update(UpdatePersonRequest model);
    }
}
