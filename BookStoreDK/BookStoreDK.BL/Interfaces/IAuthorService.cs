using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorResponse> Add(AddAuthorRequest model);
        Task<AuthorResponse> Delete(int modelId);
        Task<AuthorsCollectionResponse> GetAll();
        Task<AuthorResponse> GetById(int id);
        Task<AuthorResponse> Update(UpdateAuthorRequest model);

        Task<AuthorsCollectionResponse> AddRange(AddMultipleAuthorsRequest model);
    }
}
