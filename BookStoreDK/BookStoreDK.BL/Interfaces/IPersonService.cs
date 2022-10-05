using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Interfaces
{
    public interface IPersonService
    {
        Task<PersonResponse> Add(AddPersonRequest model);
        Task<PersonResponse> Delete(int modelId);
        Task<PersonCollectionResponse> GetAll();
        Task<PersonResponse> GetById(int id);
        Task<PersonResponse> Update(UpdatePersonRequest model);
    }
}
