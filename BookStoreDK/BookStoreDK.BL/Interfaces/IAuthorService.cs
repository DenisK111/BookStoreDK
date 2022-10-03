using BookStoreDK.Models.Models;
using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Interfaces
{
    public interface IAuthorService
    {
        AddAuthorResponse Add(AddAuthorRequest model);
        Author? Delete(int modelId);
        IEnumerable<Author> GetAll();
        Author? GetById(int id);
        UpdateAuthorResponse Update(UpdateAuthorRequest model);

        AddAuthorsResponse AddRange(AddMultipleAuthorsRequest model);
    }
}
