using BookStoreDK.Models.Requests;
using BookStoreDK.Models.Responses;

namespace BookStoreDK.BL.Interfaces
{
    public interface IBookService
    {
        Task<BookResponse> Add(AddBookRequest model);
        Task<BookResponse> Delete(int modelId);
        Task<BookCollectionResponse> GetAll();
        Task<BookResponse> GetById(int id);
        Task<BookResponse> Update(UpdateBookRequest model);

        
    }
}
