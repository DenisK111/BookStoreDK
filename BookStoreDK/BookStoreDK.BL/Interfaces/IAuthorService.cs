using BookStoreDK.Models.Models;

namespace BookStoreDK.BL.Interfaces
{
    public interface IAuthorService 
    {
        Author? Add(Author model);
        Author? Delete(int modelId);
        IEnumerable<Author> GetAll();
        Author? GetById(int id);
        Author? Update(Author model);
    }
}
